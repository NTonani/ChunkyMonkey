using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class Crocodile : MovingEntity
    {
        public int sH;
        public bool changed;
        public Crocodile(int X, int Y, double dX, double dY, int Width, int Height, int sW, int sH, bool changed) : base(X, Y, dX, dY, Width, Height, sW, sH)
        {
            this.sH = sH;
            this.changed = changed;
        }

        public override void Move()
        {
            if (X <= -(Width))
            {
                X = oldsW + Width;
                Random rand = new Random();
                dX = 0;
                Y = rand.Next(oldsH-Height,oldsH-(Height/2));
            }
            X += (int)dX;
            Y += (int)dY;
        }

        public override void Resize(int W, int H)
        {
            Width = (int)(W / 6) - 1;
            Height = (int)(H / 2) - 1;
            X = ((int)(((double)X / oldsW) * W));
            Y = ((int)(((double)Y / oldsH) * H));
            oldsW = W;
            oldsH = H;
        }

    }
}
