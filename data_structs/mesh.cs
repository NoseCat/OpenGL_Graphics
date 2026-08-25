using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public class Mesh
{
    // OpenGL handles
    private int vao;
    private int vbo;
    //private int _ebo; // Element Buffer Object (optional for indexed drawing)
    
    // Vertex data
    private Vertex[] vertices;
    //private uint[] _indices;
    
    // Properties
    //public int VertexCount => _vertices.Length;
    //public int IndexCount => _indices?.Length ?? 0;
    //public bool HasIndices => _indices != null && _indices.Length > 0;
    
    // Constructor for non-indexed meshes (like our triangle)
    public Mesh(Vertex[] vertices)
    {
        this.vertices = vertices;
        //_indices = null;
        SetupMesh();
    }
    
    // Constructor for indexed meshes
    // public Mesh(Vertex[] vertices, uint[] indices)
    // {
    //     _vertices = vertices;
    //     _indices = indices;
    //     SetupMesh();
    // }
    
    private void SetupMesh()
    {
        // Generate VAO
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        
        // Generate and setup VBO
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.Size, vertices, BufferUsageHint.StaticDraw);
        
        // // Setup EBO if indices exist
        // if (HasIndices)
        // {
        //     _ebo = GL.GenBuffer();
        //     GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        //     GL.BufferData(
        //         BufferTarget.ElementArrayBuffer, 
        //         _indices.Length * sizeof(uint), 
        //         _indices, 
        //         BufferUsageHint.StaticDraw
        //     );
        // }
        
        // Set vertex attributes
        // Position (0)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
        // in location 0 (pos), we have 3, they are floats (vector3), (they are not normalized), they takes Vertex.Size ram, no offset
        GL.EnableVertexAttribArray(0);
        
        // Color (location = 1)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Size, 3 * sizeof(float)); 
        //offset by 3 floats to skip pos
        GL.EnableVertexAttribArray(1);
        
        // Unbind
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //if (HasIndices)
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }
    
    public void Draw()
    {
        GL.BindVertexArray(vao);
        
        //if (HasIndices)
            //GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        
        GL.BindVertexArray(0);
    }
    
    public void Dispose()
    {
        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        //if (HasIndices)
            //GL.DeleteBuffer(_ebo);
    }
}

