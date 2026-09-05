// Material.cs
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Graphics;

public class Material
{
    // Texture maps
    public Texture Albedo { get; set; }
    public Texture Normal { get; set; }
    public Texture Specular { get; set; }
    public Texture Roughness { get; set; }
    public Texture AmbientOcclusion { get; set; }
    public Texture Emission { get; set; }

    // Scalar properties
    public float Shininess { get; set; } = 32.0f;
    public float Metallic { get; set; } = 0.0f;
    //public float Roughness { get; set; } = 0.5f;
    public float Opacity { get; set; } = 1.0f;
    public float EmissionStrength { get; set; } = 0.0f;

    // Color properties
    public Vector3 BaseColor { get; set; } = Vector3.One;
    public Vector3 SpecularColor { get; set; } = new Vector3(0.5f);
    public Vector3 EmissionColor { get; set; } = Vector3.Zero;

    // Constructor
    public Material(string albedoPath = "textures/default.png")
    {
        Albedo = new Texture(albedoPath);
    }

    public void Apply()
    {
        // Bind textures
        Albedo?.Bind(TextureUnit.Texture0);
        Game.shaderManager.SetUniform("texture0", 0);

        Normal?.Bind(TextureUnit.Texture1);
        Game.shaderManager.SetUniform("normalMap", 1);

        Specular?.Bind(TextureUnit.Texture2);
        Game.shaderManager.SetUniform("material.specularMap", 2);

        Roughness?.Bind(TextureUnit.Texture3);
        Game.shaderManager.SetUniform("material.roughnessMap", 3);

        // Set uniforms
        Game.shaderManager.SetUniform("material.shininess", Shininess);
        Game.shaderManager.SetUniform("material.metallic", Metallic);
        //Game.shaderManager.SetUniform("material.roughness", Roughness);
        Game.shaderManager.SetUniform("material.opacity", Opacity);
        Game.shaderManager.SetUniform("material.emissionStrength", EmissionStrength);
        Game.shaderManager.SetUniform("material.baseColor", BaseColor);
        Game.shaderManager.SetUniform("material.specularColor", SpecularColor);
        Game.shaderManager.SetUniform("material.emissionColor", EmissionColor);
    }

    public void Unbind()
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.ActiveTexture(TextureUnit.Texture2);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.ActiveTexture(TextureUnit.Texture3);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public void Dispose()
    {
        Albedo?.Dispose();
        Normal?.Dispose();
        Specular?.Dispose();
        Roughness?.Dispose();
        AmbientOcclusion?.Dispose();
        Emission?.Dispose();
    }
}