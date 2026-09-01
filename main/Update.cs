namespace Graphics;
using OpenTK.Mathematics;

partial class Game
{
    float accum = 0;
    void Update(float delta)
    {
        controller.Update(KeyboardState, MouseState, delta);

        accum += delta; 
        foreach(Light light in lightManager.lights)
        {
            light.Position = new Vector3((float)MathHelper.Sin(accum) * 2.0f, (float)MathHelper.Cos(accum) * 2.0f, light.Position.Z);
        }
    }
}
