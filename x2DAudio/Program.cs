using OpenTK.Graphics.OpenGL;
using System;


namespace x2DAudio
{
    /// <summary>
    /// 2D GameWindow Audio サンプル
    /// </summary>
    class Program : OpenTK.GameWindow
    {

        OpenTK.Audio.AudioCapture AudioCapture;
        private int[] wavebuffer512;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Program(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            //System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, 1024, 1024);

            // Opens the default device for audio recording. Implicitly set parameters are: 22050Hz, 16Bit Mono, 4096 samples ringbuffer.
            this.AudioCapture = new OpenTK.Audio.AudioCapture();

            this.AudioCapture.Start();

            Console.WriteLine("[INFO] " + this.AudioCapture.AvailableSamples);
            Console.WriteLine("[INFO] " + this.AudioCapture.CurrentDevice);
            Console.WriteLine("[INFO] " + this.AudioCapture.CurrentError);
            Console.WriteLine("[INFO] " + this.AudioCapture.SampleFormat);
            Console.WriteLine("[INFO] " + this.AudioCapture.SampleFrequency);
            Console.WriteLine("[INFO] Start Audio Capture");

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
            // wavebuffer
            wavebuffer512 = new int[1024];

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

            // copy buffer
            this.AudioCapture.ReadSamples(wavebuffer512, 1024);

        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(PrimitiveType.LineStrip);
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

            //  waveform
            GL.Begin(PrimitiveType.LineStrip);
            {
                GL.LineWidth(0.001f);
                GL.Color4(OpenTK.Graphics.Color4.White);


                for (int i = 0; i < 512; i++)
                {
                    int v = wavebuffer512[i];

                    float x0 = -1f;
                    float x1 = 1;
                    float dx = (x1 - x0) / 511f;
                    float x = (i * dx) -1 ;


                    float y0 = -2;
                    float y1 = 2;
                    float dy = (float)(y1 - y0);
                    float y = (float)v / Int32.MaxValue * dy;

                    GL.Vertex2(x, y);

                }

            }
            GL.End();

            this.SwapBuffers();

            base.OnRenderFrame(e);
        }


    }
}
