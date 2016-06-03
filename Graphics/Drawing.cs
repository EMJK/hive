using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Graphics
{
    class Drawing
    {
        public static void DrawTexture(Texture2D texture, Vector3 position)
        {
            if (position.Y > 6 || position.Y < -6 || position.Z < 0 || position.Z > 4)
            {
                MessageBox.Show("Possition incorrect!");
                return;
            }
            Vector2[] verticies = new Vector2[4]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1)
            };
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < 4; i++)
            {
                GL.TexCoord2(verticies[i]);

                int oddoffset = 0;
                if (position.Y % 2 != 0) oddoffset = 1;
                int leftborder = 0;
                if (position.X < -8) leftborder = 1;

                verticies[i].X *= texture.Width;
                verticies[i].Y *= texture.Height;
                verticies[i].X += (position.X * texture.Width) + (position.X * 4) + (oddoffset * texture.Width / 2) - (leftborder * 17) + 3;
                verticies[i].Y += (position.Y * texture.Height) + (position.Y * 14) - (position.Z * 5) - 13;

                GL.Vertex2(verticies[i]);
            }

            GL.End();
        }
        public static void DrawBg(Texture2D texture, Vector2 position)
        {
            Vector2[] verticies = new Vector2[4]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1)
            };
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < 4; i++)
            {
                GL.TexCoord2(verticies[i]);

                verticies[i].X *= texture.Width;
                verticies[i].Y *= texture.Height;
                verticies[i] += position;

                GL.Vertex2(verticies[i]);
            }

            GL.End();
        }


        public static void Begin (int screenWidth, int screenHeight)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(-screenWidth / 2f, screenWidth / 2f, screenHeight / 2f, -screenHeight / 2f, 0f, 1f);
        }
    }
}
