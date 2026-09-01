using OpenTK.Mathematics;


namespace Graphics;

partial class Game
{
    void Load()
    {
        // Light 1: Main bright white light from above-right
        Light light1 = new Light(
            position: new Vector3(3.0f, 4.0f, 3.0f),
            color: new Vector3(1.0f, 1.0f, 1.0f),
            intensity: 2.0f
        );

        // Light 2: Warm fill light from left-back
        Light light2 = new Light(
            position: new Vector3(-4.0f, 2.0f, -2.0f),
            color: new Vector3(1.0f, 0.8f, 0.6f),  // Warm orange
            intensity: 0.6f
        );

        // Light 3: Cool rim light from right-back
        Light light3 = new Light(
            position: new Vector3(4.0f, 1.0f, -4.0f),
            color: new Vector3(0.4f, 0.6f, 1.0f),  // Cool blue
            intensity: 0.4f
        );

        // Add lights to manager
        lightManager.AddLight(light1);
        lightManager.AddLight(light2);
        lightManager.AddLight(light3);

        //Model spear = new Model("models/spear.glb");
        //modelManager.Add(spear);

        Model sword = new Model("models/Sword.fbx");
        modelManager.Add(sword);
        sword.Rotation = new Vector3(0, 0, MathHelper.DegreesToRadians(90));

        Model basic_geo = new Model("models/basic_geo.fbx");
        modelManager.Add(basic_geo);
        basic_geo.Rotation = new Vector3(MathHelper.DegreesToRadians(-90), 0, 0);
        basic_geo.Position = basic_geo.Position - new Vector3(0, 0, 10);

        Model sphere = new Model("models/Sphere.fbx");
        modelManager.Add(sphere);
        sphere.Position = sphere.Position + new Vector3(5, 0, 5);
    }


}