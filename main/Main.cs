using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Graphics;

partial class Game : GameWindow
{
    static public Camera camera = new Camera(new Vector3(0, 2, 5));
    CameraController controller = new CameraController(camera);
    
    
    public static Matrix4 _modelMatrix = Matrix4.CreateTranslation(0, 0, 0); //of mesh, needs to bee in mesh class


    ShaderManager shaderManager = new();
    public Game(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine($"OpenGL: {GL.GetString(StringName.Version)}");
        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        shaderManager.Load();
        Load();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Unload();
        shaderManager.Unload();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        shaderManager.ApplyShaders();
        Render();
        
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        Update((float)args.Time);

        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            Close();
    }

    // protected override void OnResize(ResizeEventArgs e)
    // {
    //     base.OnResize(e);
    //     GL.Viewport(0, 0, e.Width, e.Height);
    // }
}


