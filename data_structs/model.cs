using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Assimp;

namespace Graphics;

public class Model
{
    private List<Mesh> meshes = new() { };

    // Transform properties
    private Matrix4 model = Matrix4.CreateTranslation(0, 0, 0);
    private Vector3 _position = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;
    private Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; UpdateModelMatrix(); }
    }
    public Vector3 Rotation
    { // in rad
        get { return _rotation; }
        set { _rotation = value; UpdateModelMatrix(); }
    }
    public Vector3 Scale
    {
        get { return _scale; }
        set { _scale = value; UpdateModelMatrix(); }
    }

    public void UpdateModelMatrix()
    {
        model = Matrix4.Identity;

        // Scale -> Rotate -> Translate
        model = Matrix4.CreateScale(Scale) * model;

        model = Matrix4.CreateRotationX(Rotation.X) * model;
        model = Matrix4.CreateRotationY(Rotation.Y) * model;
        model = Matrix4.CreateRotationZ(Rotation.Z) * model;

        model = Matrix4.CreateTranslation(Position) * model;
    }

    public void Draw()
    {
        Game.shaderManager.SetModelMatrix(model);

        foreach (Mesh mesh in meshes)
        {
            mesh.Draw();
        }
    }

    public Model(string modelPath)
    {
        using var importer = new AssimpContext();
        var scene = importer.ImportFile(modelPath, PostProcessSteps.Triangulate
                | PostProcessSteps.FlipUVs
                | PostProcessSteps.CalculateTangentSpace); //minimal to decrees the chances of failure
        if (scene == null || scene.RootNode == null)
            throw new Exception($"Failed to load model");

        Console.WriteLine($"Scene meshes:      {scene.MeshCount}");
        Console.WriteLine($"Scene materials:   {scene.MaterialCount}");
        Console.WriteLine($"Scene textures:    {scene.TextureCount}");
        Console.WriteLine($"Scene animations:  {scene.AnimationCount}");
        Console.WriteLine($"Root node:         {scene.RootNode?.Name}");
        foreach (var mesh in scene.Meshes)
        {
            //vertex info
            Console.WriteLine(
                $"Mesh '{mesh.Name}': " +
                $"vertices={mesh.VertexCount}, " +
                $"faces={mesh.FaceCount}, " +
                $"tangents={mesh.Tangents.Count}, " +
                $"uvChannels={mesh.TextureCoordinateChannelCount}"
            );
            var vertices = new Vertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var pos = mesh.Vertices[i];
                var normal = mesh.Normals[i];
                var tex = mesh.TextureCoordinateChannels[0][i];
                var tangent = mesh.Tangents[i];

                vertices[i] = new Vertex(
                    new Vector3(pos.X, pos.Y, pos.Z),
                    new Vector3(normal.X, normal.Y, normal.Z),
                    new Vector2(tex.X, tex.Y),
                    Vector3.One,
                    new Vector3(tangent.X, tangent.Y, tangent.Z)
                );
            }

            //indecies
            var indices = mesh.GetIndices().Select(i => (uint)i).ToArray(); //convert to uint

            //material     
            var material = new Material();
            var loadedMaterial = scene.Materials[mesh.MaterialIndex];
            //albedo
            if (loadedMaterial.HasTextureDiffuse)
            {
                material.Albedo = LoadTexture(scene, loadedMaterial.TextureDiffuse.FilePath);
            }
            //normal
            Console.WriteLine($"Material: {loadedMaterial.Name}");
            Console.WriteLine($"Diffuse: {loadedMaterial.HasTextureDiffuse}");
            Console.WriteLine($"Normal:  {loadedMaterial.HasTextureNormal}");

            if (loadedMaterial.HasTextureNormal)
            {
                material.Normal = LoadTexture(scene, loadedMaterial.TextureNormal.FilePath);
            }
            //scalar
            material.Shininess = loadedMaterial.Shininess; //shiness

            //add
            meshes.Add(new Mesh(vertices, material, indices));
        }
    }

    Texture LoadTexture(Scene scene, string path)
    {
        if (string.IsNullOrEmpty(path))
            return new Texture("textures/default.png");

        // Embedded texture: "*N" format
        if (path.StartsWith("*"))
        {
            int texIndex = int.Parse(path.Substring(1));
            var embTex = scene.Textures[texIndex];
            
            // embTex.CompressedData is byte[] for PNG/JPG when height == 0
            return new Texture(embTex.CompressedData);
        }

        // External file
        string fileName = Path.GetFileName(path);
        return new Texture(Path.Combine("textures", fileName));
    }
    public void Dispose()
    {
        foreach (Mesh mesh in meshes)
        {
            mesh.Dispose();
        }

    }

}