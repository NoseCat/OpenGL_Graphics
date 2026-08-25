using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class CameraController
{
    private  Camera camera;
    private Vector2 _lastMousePos = new Vector2(-1);
    public float speed = 2.5f;
    public float sensitivity = 0.1f;

    public CameraController(Camera cam)
    {
        camera = cam;
    }

    public void Update(KeyboardState keyboard, MouseState mouse, float deltaTime)
    {
        // WASD Movement
        if (keyboard.IsKeyDown(Keys.W)) Move(camera.front, deltaTime);
        if (keyboard.IsKeyDown(Keys.S)) Move(-camera.front, deltaTime);
        if (keyboard.IsKeyDown(Keys.A)) Move(-camera.right, deltaTime);
        if (keyboard.IsKeyDown(Keys.D)) Move(camera.right, deltaTime);
        if (keyboard.IsKeyDown(Keys.Space)) Move(camera.up, deltaTime);
        if (keyboard.IsKeyDown(Keys.LeftControl)) Move(-camera.up, deltaTime);

        Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
        if (_lastMousePos.X == -1) _lastMousePos = mousePos;
        MouseMovement(mousePos - _lastMousePos);
        _lastMousePos = new Vector2(mouse.X, mouse.Y);
    }

    public void Move(Vector3 direction, float delta)
    {
        camera.pos += direction * speed * delta;
    }

    public void MouseMovement(Vector2 posDelta)
    {
        camera.Yaw += posDelta.X * sensitivity;
        camera.Pitch += -posDelta.Y * sensitivity;
    }
}