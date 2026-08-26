using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

public class Light
{
    public Vector3 Position;
    public Vector3 Color;
    public float Intensity;

    public Light(Vector3 position, Vector3 color, float intensity)
    {
        Position = position;
        Color = color;
        Intensity = intensity; 
    }
}