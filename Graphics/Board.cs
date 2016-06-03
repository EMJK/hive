using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Graphics
{
    class Board : GameWindow
    {
        Texture2D background;
        Texture2D white_beetle,black_beetle;

        public Board(int width, int height)
            : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            background = ContentPipe.LoadTexture("bg.png");
            //load all the bugs' textures
            white_beetle = ContentPipe.LoadTexture("white-beetle.png");
            black_beetle = ContentPipe.LoadTexture("black-beetle.png");
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            Drawing.Begin(this.Width, this.Height);

            //drawing background
            Drawing.DrawBg(background, new Vector2((-this.Width/2f)+75,(-this.Height/2f)+25));

            //drawing bugs
            //board coordinates: -8 <= X <= 7, -6 <= Y <= 6, 0 <= Z <= 4
            //if X > 7 || X < -8: bugs outside the board
            Drawing.DrawTexture(white_beetle, new Vector3(0, 1, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(7, 2, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(3, 4, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-6, 5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-2, 6, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-4, 0, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(3, -1, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(6, -2, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(4, -3, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(2, -4, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(2, 3, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(1, -5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(7, -6, 0));
        
            Drawing.DrawTexture(black_beetle, new Vector3(8, -6, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(8, -5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, -4, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, -3, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, -2, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(8, -1, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(8, 0, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(8, 1, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, 2, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, 3, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, 4, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(8, 5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, 5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(8, 6, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-8, -6, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-8, -5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, -4, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, -3, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, -2, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, -1, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, 0, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, 1, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, 2, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, 3, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, 4, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, 5, 0));
            Drawing.DrawTexture(black_beetle, new Vector3(-9, 5, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(-9, 6, 0));
            Drawing.DrawTexture(white_beetle, new Vector3(1, -5, 1));
            Drawing.DrawTexture(black_beetle, new Vector3(1, -5, 2));
            Drawing.DrawTexture(white_beetle, new Vector3(1, -5, 3));



            this.SwapBuffers();
        }
    }
}
