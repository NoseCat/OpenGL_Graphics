using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Graphics;

class Program
{
    static public Vector2i screen_resolution = new Vector2i(800, 600);
    static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(screen_resolution.X, screen_resolution.Y),
            Title = "Graphics",
            APIVersion = new Version(4, 6), //major version, minor version
            Profile = ContextProfile.Core,
            Flags = ContextFlags.ForwardCompatible,
            WindowBorder = WindowBorder.Fixed, // I dont want to bother with that
        };

        using var game = new Game(GameWindowSettings.Default, nativeSettings);
        //using handles cleanup
        game.Run();
    }
}