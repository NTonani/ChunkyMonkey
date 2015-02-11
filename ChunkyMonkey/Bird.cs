using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class Bird : MovingEntity
    {
        public bool changed;
        public bool low = false;
        public Bird(int X, int Y, double dX, double dY, int Width, int Height, int sW, int sH, bool changed) : base(X, Y, dX, dY, Width, Height, sW, sH)
        {
            this.changed = changed;
        }

        public override void Move()
        {
            /*
            Random rand = new Random();
            if (X <= -400)
            {
                X = oldsW + 10;
                dY = rand.Next(-1, 3);
                Y = rand.Next(0,oldsH/2);
            }*/
            X += (int)dX;
            Y += (int)dY;
        }

        public override void Resize(int W, int H)
        {
            Width = (int)(W / 7) - 1;
            Height = (int)(H / 6) - 1;
            X = ((int)(((double)X / oldsW) * W));

            Y = ((int)(((double)Y / oldsH) * H));
            oldsW = W;
            oldsH = H;
        }
        /*
        public bool ExistY(MovingEntity bird2, MovingEntity monkey)
        {
            if (this.Y > bird2.Y)
            {
                if(
            }
        }*/
        public bool ProperY(MovingEntity bird2, MovingEntity monkey)
        {
            if (this.Y > bird2.Y)
            {
                if (this.Y - (bird2.Y + bird2.Height) >= monkey.Height + monkey.Height/2)
                {
                    return true;
                }
            }
            else
            {
                if (bird2.Y - (this.Y + this.Height) >= monkey.Height + monkey.Height/2)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
