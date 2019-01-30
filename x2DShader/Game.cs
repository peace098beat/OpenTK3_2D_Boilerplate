using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace x2DShaderEBO
{
    public class Game : OpenTK.GameWindow
    {

        Shader shader;
        int VertexBufferObject; // VBO
        int VertexArrayObject; // VAO


        float[] vertices =
        {
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f
        };

        /// <summary>
        /// Constractor
        /// </summary>
        public Game(int width, int height, string title) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title) { }

        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);


            // ---------- VBO
            // Bind vertices

            VertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); // VBO

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); // Position3


            // ---------- Shader

            shader = new Shader("shader.vert", "shader.frag");

            shader.Use();


            // ---------- VAO

            VertexArrayObject = GL.GenVertexArray(); // VAO

            GL.BindVertexArray(VertexArrayObject);

            GL.VertexAttribPointer(index: 0, size: 3, type: VertexAttribPointerType.Float, normalized: false, stride: 3 * sizeof(float), offset: 0);

            GL.EnableVertexAttribArray(index: 0);

            GL.BindBuffer(target: BufferTarget.ArrayBuffer, buffer: VertexBufferObject); // Bind VBO


            // ---------- 

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(mask: ClearBufferMask.ColorBufferBit);

            // ----------- VAO

            shader.Use();

            GL.BindVertexArray(VertexArrayObject); // VAO

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // ----------- 

            this.Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            OpenTK.Input.KeyboardState input = OpenTK.Input.Keyboard.GetState();

            if (input.IsKeyDown(OpenTK.Input.Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
            base.OnUnload(e);
        }
    }
}