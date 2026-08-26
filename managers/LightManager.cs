using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

public class LightManager
{
    private List<Light> lights = new List<Light>();
    private const int MAX_LIGHTS = 8;
    
    public void AddLight(Light light) { lights.Add(light); }
    //public void RemoveLight(Light light) { /* ... */ }
    
    public void Apply()
    {
        int count = Math.Min(lights.Count, MAX_LIGHTS);
        Game.shaderManager.SetUniform("lightCount", count);
        
        for (int i = 0; i < count; i++)
        {
            string prefix = $"lights[{i}]";
            Game.shaderManager.SetUniform($"{prefix}.position", lights[i].Position);
            Game.shaderManager.SetUniform($"{prefix}.color", lights[i].Color);
            Game.shaderManager.SetUniform($"{prefix}.intensity", lights[i].Intensity);
        }
    }
}