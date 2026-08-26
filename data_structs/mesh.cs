using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public class Mesh
{
    // OpenGL handles
    private int vao;
    private int vbo;
    private int ebo; // Element Buffer Object 

    // Vertex data
    private Vertex[] vertices;
    private uint[] indices;

    // Transform properties
    private Matrix4 model = Matrix4.CreateTranslation(0, 0, 0);
    private Vector3 _position = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;
    private Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; UpdateModelMatrix(); }
    }
    public Vector3 Rotation
    { // in rad
        get { return _rotation; }
        set { _rotation = value; UpdateModelMatrix(); }
    }
    public Vector3 Scale
    {
        get { return _scale; }
        set { _scale = value; UpdateModelMatrix(); }
    }


    // Constructor for non-indexed meshes
    public Mesh(Vertex[] vertices, uint[] indices = null)
    {
        this.vertices = vertices;
        if (indices == null || indices.Length == 0) //will cause issues if vertex count %3 is not 0
        {
            this.indices = new uint[vertices.Length];
            for (uint i = 0; i < vertices.Length; i++)
            {
                this.indices[i] = i;
            }
        }
        else
            this.indices = indices;

        SetupMesh();
    }

    private void SetupMesh()
    {
        // VAO
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        // VBO
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.Size, vertices, BufferUsageHint.StaticDraw);

        // EBO
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        // Set vertex attributes
        // Position (0)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
        // in location 0 (pos), we have 3, they are floats (vector3), (they are not normalized), they takes Vertex.Size ram, no offset
        GL.EnableVertexAttribArray(0);

        // Normal (1)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Size, 3 * sizeof(float));
        //offset by 3 floats to skip pos
        GL.EnableVertexAttribArray(1);

        // TexCoord (2)
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, 6 * sizeof(float));
        //offset by 3+3 floats to skip pos
        GL.EnableVertexAttribArray(2);

        // Color (3)
        GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Vertex.Size, 8 * sizeof(float));
        //offset by 3+3+2 floats to skip pos + normal + tex
        GL.EnableVertexAttribArray(3);

        // Unbind
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public void UpdateModelMatrix()
    {
        model = Matrix4.Identity;

        // Scale -> Rotate -> Translate
        model = Matrix4.CreateScale(Scale) * model;

        model = Matrix4.CreateRotationX(Rotation.X) * model;
        model = Matrix4.CreateRotationY(Rotation.Y) * model;
        model = Matrix4.CreateRotationZ(Rotation.Z) * model;

        model = Matrix4.CreateTranslation(Position) * model;
    }

    public void Draw()
    {
        Game.shaderManager.SetModelMatrix(model);

        GL.BindVertexArray(vao);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
        //GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);


        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ebo);
    }
}