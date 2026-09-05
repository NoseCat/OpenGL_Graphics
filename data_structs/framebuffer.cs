using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

class FramebufferManager
{
    public int Framebuffer { get; private set; }
    public int Texture { get; private set; }
    public int Renderbuffer { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public FramebufferManager(int width, int height)
    {
        Width = width;
        Height = height;
        CreateFramebuffer();
    }

    private void CreateFramebuffer()
    {
        // Create framebuffer
        Framebuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer);

        // Create texture to render to
        Texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, Texture, 0);

        // Create renderbuffer for depth and stencil
        Renderbuffer = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Renderbuffer);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Width, Height);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, Renderbuffer);

        // Check framebuffer status
        FramebufferErrorCode status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferErrorCode.FramebufferComplete)
            throw new Exception($"Framebuffer error: {status}");

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer);
        GL.Viewport(0, 0, Width, Height);
    }

    public void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Cleanup()
    {
        GL.DeleteFramebuffer(Framebuffer);
        GL.DeleteTexture(Texture);
        GL.DeleteRenderbuffer(Renderbuffer);
    }
}