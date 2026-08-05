using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Graphics;

class Program
{
    static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "graphics",
            APIVersion = new Version(4, 6),
            Profile = ContextProfile.Core,
            Flags = ContextFlags.ForwardCompatible,
        };

        using var window = new GameWindow(GameWindowSettings.Default, nativeSettings);
        
        window.Load += () =>
        {
            GL.ClearColor(0.1f, 0.15f, 0.25f, 1.0f);
            Console.WriteLine($"OpenGL: {GL.GetString(StringName.Version)}");
        };

        window.RenderFrame += _ =>
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            window.SwapBuffers();
        };

        window.UpdateFrame += _ =>
        {
            if (window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                window.Close();
        };

        window.Resize += e => GL.Viewport(0, 0, e.Width, e.Height);

        window.Run();
    }
}