using OpenTK.Mathematics;

namespace Graphics;

partial class Game
{
    private Mesh mesh;
    private Mesh mesh2;
    
    void Load()
    {
        //test triangle
        Vertex[] _vertices =
        [
            new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // bottom-left
            new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)), // bottom-right
            new Vertex(new Vector3(0.0f,  0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // top-center
        ];
        mesh = new Mesh(_vertices);
        Vertex[] _vertices2 =
        [
            new Vertex(new Vector3(-1f, -0.5f, 1f), new Vector3(0.0f, 1.0f, 0.0f)), // bottom-left
            new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f)), // bottom-right
            new Vertex(new Vector3(0.0f,  0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // top-center
        ];
        mesh2 = new Mesh(_vertices2);
    }
}
