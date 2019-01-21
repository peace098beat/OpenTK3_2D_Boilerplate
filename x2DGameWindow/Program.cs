using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace x2DGameWindow
{
    /// <summary>
    /// 2D GameWindowサンプル
    /// </summary>
    class Program : OpenTK.GameWindow
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Program(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, 1024, 1024);

        }

        /// <summary>
        /// メイン関数
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Antialias Mode
            using (var game = new Program(800, 600, new OpenTK.Graphics.GraphicsMode(32, 0, 0, 4), "gamewidow"))
            {
                game.Run();
            }
        }

        /// <summary>
        /// ClearColorを設定
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // Set the clear color to blue
            GL.ClearColor(0.0f, 0.0f, 1.0f, 0.0f);

            base.OnLoad(e);
        }

        /// <summary>
        /// リサイズ
        /// (座標について) ワールド座標 -> 正規化デバイス座標 -> デバイス座標系(viewport)
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            Console.WriteLine(this.ClientRectangle.ToString());
            this.Title = this.ClientRectangle.ToString();

            /* デバイス座標系での、ビューポートの位置とサイズを指定 */
            GL.Viewport(0, 0, this.Width, this.Height);

            /* 変換行列の初期化 */
            GL.LoadIdentity();

            /* ワールド座標系を正規化デバイス座標系に平行投影 (orthographic projection : 正射影) する行列 */
            /* ワールド座標系を切り取る */
            GL.Ortho(-2, 2, -2, 2, -1, 1);
            base.OnResize(e);
        }

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.LineWidth(0.001f);
                GL.Color4(OpenTK.Graphics.Color4.White);

                GL.Vertex2(0.0, 0.0);
                GL.Vertex2(1.0, 0.0);

                GL.Vertex2(1.0, 1.0);

                GL.Vertex2(-1.0, 1.0);
                GL.Vertex2(-1.0, -1.0);
                GL.Vertex2(1.0, -1.0);
                GL.Vertex2(-1.0, 1.0);

                GL.Vertex2(-1.0, -1.0);
                GL.Vertex2(1.0, 1.0);
            }
            GL.End();

            this.SwapBuffers();

            base.OnRenderFrame(e);
        }

        //static void PrintDeviceList()
        //{
        //    Console.WriteLine($"[PrintDeviceList]");

        //    IList<string> Devices = OpenTK.Audio.AudioCapture.AvailableDevices;


        //    for (int i = 0; i < Devices.Count; i++)
        //    {
        //        Console.WriteLine($"{i}:{Devices[i]}");

        //    }
        //    Console.WriteLine($"[END]");
        //}


    }
}

