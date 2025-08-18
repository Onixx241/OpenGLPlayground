using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

public class Game: GameWindow
{
    //this is my triangle
    float[] vertices = 
    {
        -0.5f,0.5f,0.0f, 0.0f,1.0f, // top left 0
        0.5f,-0.5f,0.0f, 1.0f,0.0f, // bottom right 1 
        -0.5f,-0.5f,0.0f, 0.0f,0.0f, // bottom left 2 
        0.5f,0.5f,0.0f, 1.0f,1.0f// top right 3
    };

    uint[] indices =
    {
        0,2,3,
        1,2,3
    };

    int VertexBufferObject;
    int VertexArrayObject;

    int ElementBufferObject;

    Shader shader;
    Texture tex;

    //stopwatch for oscillation
    Stopwatch newstop = new Stopwatch();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = (width, height), Title = title }) 
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);

        this.shader = new Shader("Shaders\\shader.vert", "Shaders\\shader.frag");

        //gen and bind buffers here
        this.VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(this.VertexArrayObject);

        this.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);

        this.ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        int texCoordLocation = GL.GetAttribLocation(this.shader.Handler, "aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        this.tex = new Texture("Textures\\opentexture.jpg");
        this.tex.Use();



    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        //render stuff here
        this.tex.Use();
        this.shader.Use();

        GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyReleased(Keys.Space)) 
        {
            Console.WriteLine("Space is pressed and window is closing!");
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
        Game game = new Game(600,480,"Hello TK!");

        game.Run();
    }
}