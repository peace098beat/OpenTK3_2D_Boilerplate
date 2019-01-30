using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace x2DShaderEBO
{
    public class Game : OpenTK.GameWindow
    {

        Shader shader;
        int VertexBufferObject;     // VBO
        int VertexArrayObject;      // VAO
        int ElementBufferObject;    // EBO

        // Uniform
        private Vector2 mouse;


        float[] vertices =
        {
             1.0f, 1.0f, 0.0f, //0 : top right
             1.0f,-1.0f, 0.0f, //1 : btm right
            -1.0f,-1.0f, 0.0f, //2 : btm left
            -1.0f, 1.0f, 0.0f  //3 : top left
        };

        uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public Stopwatch sw=new Stopwatch();

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

            // ---------- Uniform
            this.sw.Start();

            // ---------- VBO
            this.VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); // VBO
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw); // Position3


            // ---------- EBO
            this.ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject); // Element indices
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);


            // ---------- Shader
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();


            // ---------- VAO
            this.VertexArrayObject = GL.GenVertexArray(); // VAO
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ElementBufferObject);


            GL.VertexAttribPointer(index: 0, size: 3, type: VertexAttribPointerType.Float, normalized: false, stride: 3 * sizeof(float), offset: 0);
            GL.EnableVertexAttribArray(index: 0);

            // ---------- 

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(mask: ClearBufferMask.ColorBufferBit);

            // ---------- Uniform

            float time = (float)this.sw.Elapsed.TotalMilliseconds;
            GL.Uniform1(GL.GetUniformLocation(shader.Program, "time"), time);

            Vector2 mouse = this.mouse;
            GL.Uniform2(GL.GetUniformLocation(shader.Program, "mouse"), mouse);

            Vector2 resolution = new Vector2(Width, Height);
            GL.Uniform2(GL.GetUniformLocation(shader.Program, "resolution"), resolution);

            //vertices[0] += 0.001f;

            // ---------- VBO
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); // VBO
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw); // Position3



            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // ----------- VAO
            GL.BindVertexArray(VertexArrayObject); // VAO

            shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            // ----------- 

            this.Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            this.mouse = new Vector2(e.Mouse.X, this.Height - e.Mouse.Y);

            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.mouse = new Vector2(e.Mouse.X, this.Height - e.Mouse.Y);

            base.OnMouseUp(e);
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
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
            base.OnUnload(e);
        }
    }
}