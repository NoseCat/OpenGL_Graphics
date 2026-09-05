using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

using Graphics;

public class Texture 
{
    private int texId; 
    public TextureUnit TextureUnit { get; private set; } //which texture unit we are bound to

    //Properties
    public int Width { get; private set; }

    public int Height { get; private set; }


    // public static Texture CreateFromData(
    //     byte[] pixelData,
    //     int width,
    //     int height,
    //     bool generateMipmaps = true,
    //     TextureUnit textureUnit = TextureUnit.Texture0)
    // {
    //     return new Texture(pixelData, width, height, generateMipmaps, textureUnit);
    // }

    public Texture(string filePath, bool isSrgb = false, bool generateMipmaps = true, TextureUnit textureUnit = TextureUnit.Texture0)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Texture file not found: {filePath}");

        ImageResult image;
        using (var stream = File.OpenRead(filePath))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        Width = image.Width;
        Height = image.Height;
        TextureUnit = textureUnit;

        // Generate OpenGL textureid
        texId = GL.GenTexture();
        Bind();

        SetDefaultParameters();
        // PixelInternalFormat internalFormat;
        // if (isSrgb)
        // {
        //     internalFormat = PixelInternalFormat.SrgbAlpha;
        // }
        // else
        // {
        //     internalFormat = PixelInternalFormat.Rgba;
        // }

        // Upload pixel data to GPU
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0, // Mipmap level
            PixelInternalFormat.Rgba,
            image.Width,
            image.Height,
            0, // Border
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            image.Data);

        if (generateMipmaps)
        {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        // Unbind to avoid accidental modifications
        Unbind();
    }

    private void SetDefaultParameters()
    {
        // Texture wrapping
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // Texture filtering
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // Anisotropic filtering 
        if (GL.GetInteger(GetPName.MaxTextureMaxAnisotropy) > 1)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, 4);
        }
    }

    // From compressed bytes (PNG/JPG)
    public Texture(byte[] compressedData, bool isSrgb = false, bool generateMipmaps = true, TextureUnit textureUnit = TextureUnit.Texture0)
    {
        ImageResult image;
        using (var stream = new MemoryStream(compressedData))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        Width = image.Width;
        Height = image.Height;
        TextureUnit = textureUnit;

        texId = GL.GenTexture();
        Bind();
        SetDefaultParameters();

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            image.Width,
            image.Height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            image.Data);

        if (generateMipmaps)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        Unbind();
    }

    public void Bind()
    {
        GL.ActiveTexture(TextureUnit);
        GL.BindTexture(TextureTarget.Texture2D, texId);
    }

    public void Bind(TextureUnit textureUnit)
    {
        GL.ActiveTexture(textureUnit);
        GL.BindTexture(TextureTarget.Texture2D, texId);
    }

    public void Unbind()
    {
        GL.ActiveTexture(TextureUnit);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public void Dispose()
    {
        GL.DeleteTexture(texId);
        //GC.SuppressFinalize(this);
    }
   

}