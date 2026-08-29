namespace Graphics;
using OpenTK.Mathematics;

partial class Game
{
    void Update(float delta)
    {
        controller.Update(KeyboardState, MouseState, delta);

    }
}
