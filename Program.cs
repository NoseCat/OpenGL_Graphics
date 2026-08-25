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
            Title = "Graphics",
            APIVersion = new Version(4, 6), //major version, minor version
            Profile = ContextProfile.Core,
            Flags = ContextFlags.ForwardCompatible,
        };

        using var game = new Game(GameWindowSettings.Default, nativeSettings);
        //using handles cleanup
        game.Run();
    }
}