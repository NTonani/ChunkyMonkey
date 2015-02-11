using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    abstract class MovingEntity 
    {
        public int X;
        public int Y;
        public double dX;
        public double dY;
        public int Width;
        public int Height;
        public int oldsW;
        public int oldsH;
        public Hitbox hb;

        public MovingEntity(int X, int Y, double dX, double dY, int Width, int Height, int sW, int sH)
        {
            this.X = X;
            this.Y = Y;
            this.dX = dX;
            this.dY = dY;
            this.Width = Width;
            this.Height = Height;
            this.oldsW = sW;
            this.oldsH = sH;
        }

        public abstract void Move();
        public abstract void Resize(int W, int H);

    }
}
