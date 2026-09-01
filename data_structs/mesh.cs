using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public class Mesh
{
    private int vao;
    private int vbo;
    private int ebo;

    // Vertex data
    private Vertex[] vertices;
    private uint[] indices;

    private Material material;

    public Mesh(Vertex[] _vertices, string _texture = "textures/default.png", uint[] _indices = null)
    {

        _indices = FillIndicesIfNull(_indices);

        Material mat = new Material(_texture);

        SetupMesh(_vertices, mat, _indices);
    }

    public Mesh(Vertex[] _vertices, Material _material, uint[] _indices = null)
    {

        _indices = FillIndicesIfNull(_indices);

        SetupMesh(_vertices, _material, _indices);
    }

    private uint[] FillIndicesIfNull(uint[] _indices)
    {
        if (_indices == null || _indices.Length == 0) //will cause issues if vertex count %3 is not 0
        {
            _indices = new uint[vertices.Length];
            for (uint i = 0; i < vertices.Length; i++)
            {
                _indices[i] = i;
            }
        }  
        return _indices;
    }

    private void SetupMesh(Vertex[] vertices, Material mat, uint[] indices)
    {
        this.vertices = vertices;
        this.material = mat;
        this.indices = indices;

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

        // Tangent (4)
        GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, Vertex.Size, 11 * sizeof(float));
        //offset by 3+3+2+3 floats to skip pos + normal + tex + color
        GL.EnableVertexAttribArray(4);

        // Unbind
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }


    public void Draw()
    {
        material.Apply();

        GL.BindVertexArray(vao);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ebo);
    }
}