using OpenTK.Graphics.OpenGL;

class Shader 
{
    public int Handler;

    public Shader(string vertexPath, string fragPath) 
    {
        int vertexShader;
        int fragShader;

        string vertSource = File.ReadAllText(vertexPath);
        string fragSource = File.ReadAllText(fragPath);

        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        fragShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexShader, vertSource);
        GL.ShaderSource(fragShader, fragSource);

        GL.CompileShader(vertexShader);
        GL.CompileShader(fragShader);

        //program creation, linking shaders
        this.Handler = GL.CreateProgram();
        GL.AttachShader(Handler, vertexShader);
        GL.AttachShader(Handler, fragShader);

        GL.LinkProgram(this.Handler);
    }

    public void Use() 
    {
        GL.UseProgram(this.Handler);
    }
}