using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace x2DMandelbrot
{
    /// <summary>
    /// 2D GameWindow Audio サンプル
    /// </summary>
    class Program : OpenTK.GameWindow
    {
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

        // ワールド座標の外枠
        System.Drawing.RectangleF GlobalArea;

        public PointF gZoomCenter { get; private set; }
        public SizeF gZoomArea { get; private set; }
        public float gZoomRatio { get; private set; }
        public int IterationNum = 100;

        int N = 1024;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Program(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            this.GlobalArea = new RectangleF(0, 0, N, N);

        }


        float[] image;

        /// <summary>
        /// ClearColorを設定
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);

            image = new float[N * N];

            //this.gZoomCenter = new PointF(0, 0);
            //this.gZoomArea = new SizeF(4, 4);
            //this.gZoomRatio = 0.8F;

            //var x = gZoomCenter.X - gZoomArea.Width / 2f;
            //var y = gZoomCenter.Y - gZoomArea.Height / 2f;
            //var w = gZoomArea.Width;
            //var h = gZoomArea.Height;
            //this.GlobalArea = new RectangleF(x, y, w, h);
        }


        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            this.IterationNum += e.Delta * 5;

            this.Title = $"{IterationNum}, {e.Delta}";

        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // グローバル座標でのマウスポイント位置
            float new_Cx = (GlobalArea.Width) * ((float)e.X / Width) + GlobalArea.Left;
            float new_Cy = (GlobalArea.Height) * ((float)e.Y / Height) + GlobalArea.Top;
            gZoomCenter = new PointF(new_Cx, new_Cy);

            var g = GlobalArea;

            // クリップ矩形サイズを再計算
            gZoomRatio = 0.9f;
            gZoomArea = new SizeF(gZoomArea.Width * gZoomRatio, gZoomArea.Height * gZoomRatio);

            // マウス周辺のグローバル座標 おまじない
            var x = gZoomCenter.X - gZoomArea.Width / 2f;
            var y = gZoomCenter.Y - gZoomArea.Height / 2f;
            var w = gZoomArea.Width;
            var h = gZoomArea.Height;
            this.GlobalArea = new RectangleF(x, y, w, h);

            /* デバイス座標系での、ビューポートの位置とサイズを指定 */
            /* ワールド座標系を切り取る */
            GL.LoadIdentity();
            GL.Ortho(GlobalArea.Left, GlobalArea.Right, GlobalArea.Bottom, GlobalArea.Top, -1, 1);

        }


        /// <summary>
        /// リサイズ
        /// (座標について) ワールド座標 -> 正規化デバイス座標 -> デバイス座標系(viewport)
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            /* デバイス座標系での、ビューポートの位置とサイズを指定 */
            GL.Viewport(0, 0, this.Width, this.Height);

            /* 変換行列の初期化 */
            GL.LoadIdentity();

            /* ワールド座標系を切り取る */
            GL.Ortho(GlobalArea.Left, GlobalArea.Right, GlobalArea.Bottom, GlobalArea.Top, -1, 1);

            base.OnResize(e);
        }

        /// <summary>
        /// キーボードショートカット
        /// </summary>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            var g = GlobalArea;

            // クリップ矩形サイズを再計算
            gZoomRatio = 0.8f;
            gZoomArea = new SizeF(gZoomArea.Width * gZoomRatio, gZoomArea.Height * gZoomRatio);

            // マウス周辺のグローバル座標 おまじない
            var x = gZoomCenter.X - gZoomArea.Width / 2f;
            var y = gZoomCenter.Y - gZoomArea.Height / 2f;
            var w = gZoomArea.Width;
            var h = gZoomArea.Height;
            this.GlobalArea = new RectangleF(x, y, w, h);

            /* デバイス座標系での、ビューポートの位置とサイズを指定 */
            /* ワールド座標系を切り取る */
            GL.LoadIdentity();
            GL.Ortho(GlobalArea.Left, GlobalArea.Right, GlobalArea.Bottom, GlobalArea.Top, -1, 1);
        }


        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Console.WriteLine("On Update");

            count++;
            //image[count] = count / (float)(N*N);

            if (count > N * N) count = 0;
            image[count] = 1f;

        }

        int count = 0;
        //int xi = 0;
        //int yi = 0;
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            //Console.WriteLine($"xi:{xi}, yi:{yi}, c:{count}");
            GL.Begin(PrimitiveType.Points);
            {
                for (int i = 0; i < N*N; i++)
                {
                    int x = (int)((float)i % N);
                    int y = (int)((float)i / N);
                    //float c = image[i];
                    float c = 1f;
                    GL.Color3(c, 0f, 0f);
                    GL.Vertex2(x,y);
                    //Console.WriteLine($"xi:{x}, yi:{y}, c:{c}");
                }

            }
            GL.End();
            this.SwapBuffers();
            base.OnRenderFrame(e);
        }

   
        void DrawMandelBrot()
        {
            Console.WriteLine("■ 描画開始");
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // Set the clear color to blue
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // まんでるぶろ集合の計算
            GL.Begin(PrimitiveType.Points);
            {
                var rect = GlobalArea;

                int Iter = this.IterationNum;

                int gridx = this.Width;
                int gridy = this.Height;

                double dx = (double)rect.Width / gridx;
                double dy = (double)rect.Height / gridy;

                double lx = dx * gridx;
                double ly = dy * gridy;

                double Zr = 0;
                double Zi = 0;

                for (int ix = 0; ix < gridx; ix++)
                {
                    double Cr = (dx * ix) + rect.Left;

                    for (int iy = 0; iy < gridy; iy++)
                    {
                        double Ci = (dy * iy) + rect.Top;

                        // 収束計算
                        Zr = 0;
                        Zi = 0;
                        double AbsZ = 0;
                        int i = 0;
                        for (i = 0; i < Iter; i++)
                        {
                            double new_Zr = (Zr * Zr - Zi * Zi) + Cr;
                            double new_Zi = (2 * Zr * Zi + Ci);
                            Zr = new_Zr;
                            Zi = new_Zi;

                            AbsZ = (new_Zr * new_Zr + new_Zi * new_Zi);

                            if (AbsZ > 4)
                            {
                                AbsZ = 4.0;
                                break;
                            }
                        }

                        double iter = (double)i / Iter;

                        GL.Color3(1 - (AbsZ / 4.0), 1.0, iter);
                        GL.Vertex2(Cr, Ci);
                    }
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            {
                GL.Color3(0.5, 0.5, 0.5);

                var g = this.GlobalArea;
                GL.Vertex2(g.Left, gZoomCenter.Y);
                GL.Vertex2(g.Right, gZoomCenter.Y);

                GL.Vertex2(gZoomCenter.X, g.Top);
                GL.Vertex2(gZoomCenter.X, g.Bottom);
            }
            GL.End();



            // 結果表示
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine($"■ 処理Aにかかった時間:　{ts}");

            Console.WriteLine($"{GlobalArea.ToString()}");
        }

    }
}
