using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
using System.Timers;

public class Game : GameWindow 
{

    float[] vertices = 
    {
        -0.5f,0.5f,0.0f,//left top 
        -0.5f,-0.5f,0.0f,//left bottom
        0.5f,-0.5f,0.0f,//right bottom
        0.5f,0.5f,0.0f//right top
    };

    uint[] indices =
    {
        0,1,3,
        1,2,3
    };
    private int VertexArrayObject;
    private int VertexBufferObject;
    private int ElementArrayObject;

    private Shader _shader;
    private Stopwatch _timer { get; set; } = new Stopwatch();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize=(width,height), Title = title }) 
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.3f, 0.2f, 0.5f, 1.0f);

        //buffer stuff here 
        this.VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(this.VertexArrayObject);

        this.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);

        this.ElementArrayObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrayObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);
        
        //with BufferData, my vertices are uploaded in the GPU buffer
        this.vertices = null;
        //This WORKS !
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

        //put shader here
        this._shader = new Shader("Shaders\\shader.vert", "Shaders\\shader.frag");

        this._timer.Start();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        //code here.
        this._shader.Use();

        double timeVal = _timer.Elapsed.TotalSeconds;
        float greenVal = (float)Math.Sin(timeVal) / 2.0f + 0.5f;
        int vertexColorLocation = GL.GetUniformLocation(this._shader.handle, "uniColor");
        GL.Uniform4(vertexColorLocation, 1.0f, greenVal, 0.0f, 1.0f);


        GL.BindVertexArray(VertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);


        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyReleased(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space)) 
        {
            Console.WriteLine("Space is pressed!");
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}

public class Program 
{
    public static void Main() 
    {
        Game game = new Game(800,600,"Hello Triangle!");
        game.Run();
    }
}
