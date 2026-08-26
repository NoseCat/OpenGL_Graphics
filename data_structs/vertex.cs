using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoord;
    public Vector3 Color;
    
    public static int Size => 11 * sizeof(float); // 3 position + 3 normal + 2 texcoord + 3 color
    
    public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord, Vector3 color)
    {
        Position = position;
        Normal = normal;
        TexCoord = texCoord;
        Color = color;
    }
}