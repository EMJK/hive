using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;

namespace Graphics
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Board window = new Board(800, 600);
            window.Run();
        }
    }
}
