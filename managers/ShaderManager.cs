using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

class ShaderManager
{
    public int currentPass;
    public int basicPass;
    private Dictionary<string, Shader> basic = new() { 
        { "vertex", new Shader("Shaders/base/base_vertex.glsl", ShaderType.VertexShader) } ,
        { "fragment", new Shader("Shaders/base/base_fragment.glsl", ShaderType.FragmentShader) }
        };

    public int additionalPass;
    private Dictionary<string, Shader> additional = new() { 
        { "vertex", new Shader("Shaders/base/base_vertex.glsl", ShaderType.VertexShader) },
        //{ "fragment", new Shader("Shaders/base/default_fragment.glsl", ShaderType.FragmentShader) }
        };

    private FramebufferManager framebuffer;
    private int screenWidth, screenHeight;

    //public void AddShader(string path, ShaderType type)
    //{
     //   basic.Add(new Shader(path, type));
    //}

    public ShaderManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;
        //framebuffer = new FramebufferManager(width, height);
    }

    public void SetShader(string path, ShaderType type)
    {
        if(type == ShaderType.VertexShader)
            additional["vertex"] = new Shader(path, type);
        if(type == ShaderType.FragmentShader)
            additional["fragment"] = new Shader(path, type);
    }

    private int CreateProgram(Dictionary<string, Shader> program)
    {
        int program_id = GL.CreateProgram();
        foreach (Shader shader in program.Values)
            GL.AttachShader(program_id, shader.Compile());
        GL.LinkProgram(program_id);

        GL.GetProgram(program_id, GetProgramParameterName.LinkStatus, out int ok);
        if (ok == 0)
            throw new Exception($"Link error: {GL.GetProgramInfoLog(program_id)}");

        foreach (Shader shader in program.Values)
            shader.Clear();

        return program_id;
    }

    public void Load()
    {
        basicPass = CreateProgram(basic);
        currentPass = basicPass;
        additionalPass = CreateProgram(additional);
    }

    public void Unload()
    {
        GL.DeleteProgram(basicPass);
        GL.DeleteProgram(additionalPass);
        //framebuffer.Cleanup();
    }

    public void ApplyShaders()
    {
        //framebuffer.Bind();
        //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        currentPass = basicPass;
        GL.UseProgram(basicPass);
        SetViewMatrix(Game.camera);
        SetProjectionMatrix(Game.camera);
        SetUniform("viewPos", Game.camera.pos);

        //framebuffer.Unbind();
        //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        //framebuffer.Bind();
        //GL.UseProgram(additionalPass);

    }

    public void SetViewMatrix(Camera camera)
    {
        SetUniform("view", camera.GetViewMatrix());

    }
    public void SetProjectionMatrix(Camera camera)
    {
        SetUniform("projection", camera._projectionMatrix);
    }


    public void SetModelMatrix(Matrix4 model)
    {
        SetUniform("model", model);
    }

    public void SetModelMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Matrix4 model = Matrix4.CreateScale(scale) *
                        Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) *
                        Matrix4.CreateTranslation(position);
        SetUniform("model", model);
    }

    // Set uniform
    public void SetUniform(string name, int value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, float value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform2(location, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform3(location, value);
    }

    public void SetUniform(string name, Vector4 value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform4(location, value);
    }

    public void SetUniform(string name, Matrix4 value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.UniformMatrix4(location, false, ref value);
    }

    public void SetUniform(string name, bool value)
    {
        int location = GL.GetUniformLocation(currentPass, name);
        GL.Uniform1(location, value ? 1 : 0);
    }
}
