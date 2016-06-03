using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Graphics
{
    class ContentPipe
    {
        public static Texture2D LoadTexture (string path)
        {
            if (!File.Exists("Textures/" + path))
                throw new FileNotFoundException("File not found!");

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap("Textures/" + path);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            return new Texture2D(id, bmp.Width, bmp.Height);
        }
    }
    struct Texture2D
    {
        private int id;
        private int width, height;

        public int ID
        {
            get { return id; }
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public Texture2D(int id, int width, int height)
        {
            this.id = id;
            this.width = width;
            this.height = height;
        }
    }

}
