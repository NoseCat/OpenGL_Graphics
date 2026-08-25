using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Graphics;

partial class Game : GameWindow
{
    private int _shaderProgram;

    public Game(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine($"OpenGL: {GL.GetString(StringName.Version)}");
        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        // === Shaders ===
        _shaderProgram = CreateShaderProgram("Shaders/shader.vert", "Shaders/shader.frag");

        Load();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Unload();
        GL.DeleteProgram(_shaderProgram);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.UseProgram(_shaderProgram);
        Render();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Update();

        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            Close();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private static int CreateShaderProgram(string vertPath, string fragPath)
    {
        int vert = CompileShader(ShaderType.VertexShader, File.ReadAllText(vertPath));
        int frag = CompileShader(ShaderType.FragmentShader, File.ReadAllText(fragPath));

        int program = GL.CreateProgram();
        GL.AttachShader(program, vert);
        GL.AttachShader(program, frag);
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int ok);
        if (ok == 0)
            throw new Exception($"Link error: {GL.GetProgramInfoLog(program)}");

        GL.DeleteShader(vert);
        GL.DeleteShader(frag);

        return program;
    }

    private static int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int ok);
        if (ok == 0)
            throw new Exception($"Compile error ({type}): {GL.GetShaderInfoLog(shader)}");

        return shader;
    }
}


