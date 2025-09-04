using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using StbImageSharp;
using System.Runtime.CompilerServices;

public class MainClas : GameWindow 
{
    float[] vertices = 
    {
        0.5f,0.5f,0.0f,
        0.5f,-0.5f,0.0f,
        -0.5f,-0.5f,0.0f,
        -0.5f,0.5f,0.0f
    };

    uint[] indices = 
    {
        0,1,2,
        0,2,3
    };

    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;

    Shader newShade;

    public MainClas(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = (width, height), Title = title })
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.5f,0.2f,0.1f,1.0f);

        this.newShade = new Shader("Shaders\\shader.vert", "Shaders\\shader.frag");
        this.newShade.CompileShaders();


        this.VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(this.VertexArrayObject);

        this.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer,this.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);

        this.ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

        //this still works because once you use bufferdata the data is in your gpu
        this.VertexBufferObject = 0;

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);


        newShade.Use();

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        newShade.Use();
        GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyReleased(Keys.Space)) 
        {

            SetVertices([0.1f, 0.2f, 0.0f,
        0.1f, -0.3f, 0.0f,
        -0.1f, -0.2f, 0.0f,
        -0.2f, 0.3f, 0.0f]);

        }

        if (KeyboardState.IsKeyReleased(Keys.Period)) 
        {
            SetVertices([0.5f,0.5f,0.0f,
        0.5f,-0.5f,0.0f,
        -0.5f,-0.5f,0.0f,
        -0.5f,0.5f,0.0f]);

        }

        if (KeyboardState.IsKeyReleased(Keys.Apostrophe)) 
        {
            MoveLeft();
        }

    }

    public float[] SetVertices(float[] vertices) 
    {
        this.vertices = vertices;
        foreach (var vertex in this.vertices)
            Console.WriteLine(vertex + "\n----");

        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);
        return vertices;
    }

    public void SetIndices(uint[] indices) 
    {
        this.indices = indices;
    }

    //this definitley does NOT move left lol
    public void MoveLeft() 
    {
        float[] LeftArr = 
        [-0.01f,-0.01f,0.0f,
        -0.01f,-0.01f,0.0f,
        -0.01f,-0.01f,0.0f,
        -0.01f,-0.01f,0.0f];

        float[] newarr;

        for (int i = 0; i < vertices.Length - 1; i++) 
        {
            vertices[i] = vertices[i] - LeftArr[i];
        }

        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);


        foreach (var vertex in this.vertices)
            Console.WriteLine(vertex + "\n----");

    }
}

class Program 
{
    public static void Main() 
    {
        MainClas game = new MainClas(600, 480, "Opentk");

        game.Run();
    }
}
