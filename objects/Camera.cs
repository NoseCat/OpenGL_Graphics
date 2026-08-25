using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using Graphics;

public class Camera
{
    // Camera vectors
    public Vector3 pos;

    public Vector3 front {get; private set;}
    public Vector3 up {get; private set;}
    public Vector3 right {get; private set;}

    // Euler angles
    private float yaw = -90f;
    private float pitch = 0f;

    // Camera options
    public float fov = 60f;
    private float aspectRatio = Program.screen_resolution.X / Program.screen_resolution.Y;


    public Matrix4 _projectionMatrix;
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(pos, pos + front, up);
    }

    //private bool _matricesDirty = true; //optimization, only recompute matrices when they are actually change
    //set to true in Setters: pos, yaw, pitch, zoom, Funcs:  Move, ProcessMouseMovement
    //check for in Getters: viewMatrix, 

    public Camera(Vector3 position, float _fov = 90f)
    {
        pos = position;
        fov = _fov;

        front = -Vector3.UnitZ;
        up = Vector3.UnitY;
        right = Vector3.UnitX;


        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(fov),
            aspectRatio,
            0.1f, //near plane
            1000f //far plane
        );

        UpdateBasis();
    }

    // Yaw and Pitch update basis
    public float Yaw
    {
        get => yaw;
        set
        {
            yaw = value;
            UpdateBasis();
        }
    }

    public float Pitch
    {
        get => pitch;
        set
        {
            pitch = MathHelper.Clamp(value, -89f, 89f);
            UpdateBasis();
        }
    }



    // Update camera basis vectors from Euler angles
    private void UpdateBasis()
    {
        // Calculate front vector from yaw and pitch
        Vector3 front;
        front.X = MathF.Cos(MathHelper.DegreesToRadians(yaw)) *
                  MathF.Cos(MathHelper.DegreesToRadians(pitch));
        front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
        front.Z = MathF.Sin(MathHelper.DegreesToRadians(yaw)) *
                  MathF.Cos(MathHelper.DegreesToRadians(pitch));
        this.front = Vector3.Normalize(front);

        // Calculate right and up vectors
        right = Vector3.Normalize(Vector3.Cross(this.front, Vector3.UnitY));
        up = Vector3.Normalize(Vector3.Cross(right, this.front));
    }
}