using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Assimp;

namespace Graphics;

public class Model
{
    private List<Mesh> meshes = new(){};

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
        //var modelPath = "models/Sword.fbx"; //.glb doesnt work, use .fbx
        using var importer = new AssimpContext();
        var scene = importer.ImportFile(modelPath, 
            PostProcessSteps.Triangulate | 
            PostProcessSteps.GenerateSmoothNormals |
            PostProcessSteps.FlipUVs |
            PostProcessSteps.CalculateTangentSpace);
        if (scene == null || scene.RootNode == null)
            throw new Exception($"Failed to load model");

        foreach (var mesh in scene.Meshes)
        {
            //vertex info
            var vertices = new Vertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var pos = mesh.Vertices[i];
                var normal = mesh.Normals.Count > i ? mesh.Normals[i] : new Vector3D(0, 1, 0);
                var tex = mesh.TextureCoordinateChannels[0]?.Count > i ? mesh.TextureCoordinateChannels[0][i] : new Vector3D(0, 0, 0);
                
                vertices[i] = new Vertex(
                    new Vector3(pos.X, pos.Y, pos.Z),
                    new Vector3(normal.X, normal.Y, normal.Z),
                    new Vector2(tex.X, tex.Y),
                    Vector3.One
                );
            }

            //indecies
            var indices = mesh.GetIndices().Select(i => (uint)i).ToArray(); //covert to uint

            //textures            
            string texturePath = "textures/default.png";
            //string textureDir = "textures/";
            //if (mesh.MaterialIndex >= 0 && scene.Materials.Count > mesh.MaterialIndex)
            {
                var material = scene.Materials[mesh.MaterialIndex];
                //if (material.HasTextureDiffuse)
                {
                    texturePath = material.TextureDiffuse.FilePath;
                    //if (!string.IsNullOrEmpty(texPath))
                        //texturePath = Path.Combine(textureDir, Path.GetFileName(texPath));
                }
            }

            meshes.Add(new Mesh(vertices, texturePath, indices));
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