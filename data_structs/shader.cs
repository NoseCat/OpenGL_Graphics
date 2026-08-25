using OpenTK.Graphics.OpenGL4;

namespace Graphics;

class Shader
{
    public ShaderType type;
    public string path;

    private bool compiled = false;
    private int compiled_shader;    

    public Shader(string _path, ShaderType _type)
    {
        path = _path;
        type = _type;
    }

    public void Clear()
    {
        if(compiled)
        {
            GL.DeleteShader(compiled_shader);
            compiled = false;
        }
    }

    public int Compile()
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, File.ReadAllText(path));
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int ok);
        if (ok == 0)
            throw new Exception($"Compile error ({type}): {GL.GetShaderInfoLog(shader)}");

        compiled = true;
        compiled_shader = shader;
        return compiled_shader;
    }
    
}
