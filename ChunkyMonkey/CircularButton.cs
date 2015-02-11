using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class CircularButton
    {
        public int X;
        public int Y;
        public int Height;
        public int Width;
        public int oldsH;
        public int oldsW;

        public CircularButton(int X, int Y, int Height, int Width, int oldsH, int oldsW)
        {
            this.X = X;
            this.Y = Y;
            this.Height = Height;
            this.Width = Width;
            this.oldsH = oldsH;
            this.oldsW = oldsW;
        }
    }
}
