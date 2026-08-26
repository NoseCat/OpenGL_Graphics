using OpenTK.Mathematics;

namespace Graphics;

partial class Game
{
    void Load()
    {
        // Test triangle with proper normals (facing towards +Z)
        Vertex[] _vertices =
        [
            new Vertex(
                new Vector3(-0.5f, -0.5f, 0.0f),  // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(0.0f, 0.0f),          // TexCoord
                new Vector3(1.0f, 1.0f, 1.0f)     // Color (white)
            ),
            new Vertex(
                new Vector3(0.5f, -0.5f, 0.0f),   // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(1.0f, 0.0f),          // TexCoord
                new Vector3(1.0f, 0.0f, 0.0f)     // Color (red)
            ),
            new Vertex(
                new Vector3(0.0f, 0.5f, 0.0f),    // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(0.5f, 1.0f),          // TexCoord
                new Vector3(1.0f, 1.0f, 1.0f)     // Color (white)
            ),
        ];
        Mesh mesh = new Mesh(_vertices);

        // Second triangle with proper normals (facing towards +Z and slightly rotated)
        Vertex[] _vertices2 =
        [
            new Vertex(
                new Vector3(-1.0f, -0.5f, 1.0f),  // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(0.0f, 0.0f),          // TexCoord
                new Vector3(0.0f, 1.0f, 0.0f)     // Color (green)
            ),
            new Vertex(
                new Vector3(0.5f, -0.5f, 0.0f),   // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(1.0f, 0.0f),          // TexCoord
                new Vector3(1.0f, 1.0f, 0.0f)     // Color (yellow)
            ),
            new Vertex(
                new Vector3(0.0f, 0.5f, 0.0f),    // Position
                new Vector3(0.0f, 0.0f, 1.0f),    // Normal (pointing towards camera)
                new Vector2(0.5f, 1.0f),          // TexCoord
                new Vector3(1.0f, 1.0f, 1.0f)     // Color (white)
            ),
        ];
        Mesh mesh2 = new Mesh(_vertices2);

        // Position the second mesh differently so they don't overlap completely
        mesh2.Position = new Vector3(0.0f, 0.0f, 0.5f);

        Game.meshManager.Add(mesh);
        Game.meshManager.Add(mesh2);

        // Light 1: Main bright white light from above-right
        Light light1 = new Light(
            position: new Vector3(3.0f, 4.0f, 3.0f),
            color: new Vector3(1.0f, 1.0f, 1.0f),
            intensity: 2.0f
        );
        
        // Light 2: Warm fill light from left-back
        Light light2 = new Light(
            position: new Vector3(-4.0f, 2.0f, -2.0f),
            color: new Vector3(1.0f, 0.8f, 0.6f),  // Warm orange
            intensity: 0.6f
        );
        
        // Light 3: Cool rim light from right-back
        Light light3 = new Light(
            position: new Vector3(4.0f, 1.0f, -4.0f),
            color: new Vector3(0.4f, 0.6f, 1.0f),  // Cool blue
            intensity: 0.4f
        );
        
        // Add lights to manager
        lightManager.AddLight(light1);
        lightManager.AddLight(light2);
        lightManager.AddLight(light3);
    }
}