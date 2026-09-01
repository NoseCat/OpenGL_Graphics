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


    private int CountMeshes(Node node, Scene scene)
    {
        int count = 0;
        foreach (var meshIndex in node.MeshIndices)
            count++;
        foreach (var child in node.Children)
            count += CountMeshes(child, scene);
        return count;
    }

    public Model(string modelPath)
    {
        using var importer = new AssimpContext();
        var scene = importer.ImportFile(modelPath, PostProcessSteps.Triangulate
                | PostProcessSteps.FlipUVs);
        if (scene == null || scene.RootNode == null)
            throw new Exception($"Failed to load model");

        Console.WriteLine($"Loaded {scene.MeshCount} meshes.");
        Console.WriteLine(CountMeshes(scene.RootNode, scene));
        foreach (var mesh in scene.Meshes)
        {
            //vertex info
            var vertices = new Vertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var pos = mesh.Vertices[i];
                var normal = mesh.Normals.Count > i ? mesh.Normals[i] : new Vector3D(0, 1, 0);
                var tex = mesh.TextureCoordinateChannels[0]?.Count > i ? mesh.TextureCoordinateChannels[0][i] : new Vector3D(0, 0, 0);
                var tangent = mesh.Tangents.Count > i ? mesh.Tangents[i] : new Vector3D(1, 0, 0);

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
            var loadedMaterial = scene.Materials[mesh.MaterialIndex];
            var material = new Material();
            //albedo
            if (loadedMaterial.HasTextureDiffuse)
            {
                string texturePath = "textures/default.png";
                texturePath = loadedMaterial.TextureDiffuse.FilePath;
                string fileName = Path.GetFileName(texturePath);
                material.Albedo = new Texture(Path.Combine("textures", fileName));
            }
            //normal
            System.Console.WriteLine(loadedMaterial.HasTextureNormal);
            if (loadedMaterial.HasTextureNormal)
            {
                string texturePath = loadedMaterial.TextureNormal.FilePath;
                string fileName = Path.GetFileName(texturePath);
                material.Normal = new Texture(Path.Combine("textures", fileName));
            }
            //scalar
            material.Shininess = loadedMaterial.Shininess; //shiness

            //add
            meshes.Add(new Mesh(vertices, material, indices));
        }
    }
    public void Dispose()
    {
        foreach (Mesh mesh in meshes)
        {
            mesh.Dispose();
        }

    }

}