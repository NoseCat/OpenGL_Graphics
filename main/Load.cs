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

        Model sword = new Model("models/Sword.fbx");
        modelManager.Add(sword);
    }

   
}