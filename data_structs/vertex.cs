using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoord;
    public Vector3 Color;
    public Vector3 Tangent;
    
    public static int Size => 14 * sizeof(float); // 3 position + 3 normal + 2 texcoord + 3 color + 3 tangent
    
    public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord, Vector3 color, Vector3 tangent)
    {
        Position = position;
        Normal = normal;
        TexCoord = texCoord;
        Color = color;
        Tangent = tangent;
    }
}