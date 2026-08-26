using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

class MeshManager
{
    private List<Mesh> meshes = new() {};

    public void Add(Mesh mesh)
    {
        meshes.Add(mesh);
    }

    public void Draw()
    {
        foreach (Mesh mesh in meshes)
        {
            mesh.Draw();
        }
    }

    public void Dispose()
    {
        foreach (Mesh mesh in meshes)
        {
            mesh.Dispose();            
        } 
        meshes.Clear();
    }
}
