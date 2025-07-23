using OpenTK.Graphics.OpenGL;

public class Shader 
{
    public int handle;

    public Shader(string vertexPath, string fragmentPath) 
    {
        int vertexShader;
        int fragmentShader;

        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);

        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);

        fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);

        GL.CompileShader(vertexShader);
        GL.CompileShader(fragmentShader);

        //program
        this.handle = GL.CreateProgram();

        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);

        GL.LinkProgram(handle);

        //thats it unless i detach and delete shaders which is recommended, also should implement IDisposable.
    }

    public void Use() 
    {
        GL.UseProgram(handle);
    }
}