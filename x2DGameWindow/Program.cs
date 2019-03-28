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
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

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
            //GL.Ortho(-2, 2, -2, 2, -1, 1);
            GL.Ortho(-5, 105, -5, 105, -1, 1);
            base.OnResize(e);
        }

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        float ASin(float t)
        {
            return (float)((Math.Sin(t)+1.0)/2.0);
        }
        int count;
        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            count++;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            float t = ASin(count / 100.0f);

            float Lx = 100;
            float Ly = 100;
            int Nx = 128;
            int Ny = 128;

            float dx = Lx/Nx;
            float dy = Ly/Ny;

            System.Random r = new System.Random();

            this.Title = $"N={Nx*Ny}";

            GL.Begin(PrimitiveType.Quads);
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        // Recti,j
                        float x0 = i * dx + dx * 0.1f;
                        float y0 = j * dy + dy * 0.1f;
                        float x1 = x0 + dx - dx * 0.1f;
                        float y1 = y0 + dy - dy * 0.1f;

                        float h = (float)(t + r.Next(-3,3)/3.0f);
                        if (1.0 < h) h -= 1.0f;
                        else if (h < 0) h = 1 - h;
                        
                        var c = OpenTK.Graphics.Color4.FromHsv(new OpenTK.Vector4( h, 1.0f, 1.0f, 1f));
                        GL.Color4(c);

                        GL.Vertex2(x0, y0);

                        GL.Vertex2(x1, y0);

                        GL.Vertex2(x1, y1);

                        GL.Vertex2(x0, y1);
                    }
                }


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

