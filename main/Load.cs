using OpenTK.Mathematics;

namespace Graphics;

partial class Game
{
    //test triangle
    private readonly Vertex[] _vertices =
    [
        new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // bottom-left
        new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // bottom-right
        new Vertex(new Vector3(0.0f,  0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)), // top-center
    ];

    private Mesh mesh;
    
    void Load()
    {
        mesh = new Mesh(_vertices);
    }
}
