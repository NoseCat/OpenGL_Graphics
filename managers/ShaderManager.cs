using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

class ShaderManager
{
    private int program;
    private List<Shader> shaders = new() { //TODO: Maybe seperate basic shaders into a seperate category?
        new Shader("Shaders/shader.vert", ShaderType.VertexShader),
        new Shader("Shaders/shader.frag", ShaderType.FragmentShader)
        };

    public void AddShader(string path, ShaderType type)
    {
        shaders.Add(new Shader(path, type));
    }

    private void CreateProgram()
    {
        program = GL.CreateProgram();
        foreach (Shader shader in shaders)
            GL.AttachShader(program, shader.Compile());
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int ok);
        if (ok == 0)
            throw new Exception($"Link error: {GL.GetProgramInfoLog(program)}");

        foreach (Shader shader in shaders)
            shader.Clear();
    }

    public void Load()
    {
        CreateProgram();
    }

    public void Unload()
    {
        GL.DeleteProgram(program);
    }

    public void ApplyShaders()
    {
        GL.UseProgram(program);
        SetViewMatrix(Game.camera);
        SetProjectionMatrix(Game.camera);
        SetUniform("lightPos", new Vector3(0, 2, 5));
        SetUniform("lightColor", new Vector3(1,0,0));
        SetUniform("viewPos", Game.camera.pos);
    }

    public void SetViewMatrix(Camera camera)
    {
        SetUniform("view", camera.GetViewMatrix());

    }
    public void SetProjectionMatrix(Camera camera)
    {
        SetUniform("projection", camera._projectionMatrix);
    }
        //SetUniform("viewPos", camera.pos);


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
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, float value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform2(location, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform3(location, value);
    }

    public void SetUniform(string name, Vector4 value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform4(location, value);
    }

    public void SetUniform(string name, Matrix4 value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.UniformMatrix4(location, false, ref value);
    }

    public void SetUniform(string name, bool value)
    {
        int location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value ? 1 : 0);
    }
}
