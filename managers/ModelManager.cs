using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Graphics;

class ModelManager
{
    public List<Model> models = new() {};

    public void Add(Model model)
    {
        models.Add(model);
    }

    public void Draw()
    {
        foreach (Model model in models)
        {
            model.Draw();
        }
    }

    public void Dispose()
    {
        foreach (Model model in models)
        {
            model.Dispose();            
        } 
        models.Clear();
    }
}
