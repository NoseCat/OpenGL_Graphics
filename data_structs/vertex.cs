using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Color;
    
    public static int Size => 6 * sizeof(float); // 3 for position + 3 for color
    
    public Vertex(Vector3 position, Vector3 color)
    {
        Position = position;
        Color = color;
    }
}