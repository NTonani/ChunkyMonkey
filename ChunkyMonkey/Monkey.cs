using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class Monkey : MovingEntity
    {
        public double ddY;

        public Monkey(int X, int Y, double dX, double dY, double ddY, int Width, int Height, int sW, int sH) : base(X, Y, dX, dY, Width, Height, sW, sH)
        {
            this.ddY = ddY;
        }

        public void Swing()
        {
            if (dY > 6)
            {
                dY = 6;
            }
            ddY = -.4;
        }

        public void Fall()
        {
            if (dY < -6)
                dY = -6;
            ddY = .4;
        }
        
        public override void Move()
        {
            dY += ddY;

            X += (int)dX;
            Y += (int)dY;

            if (dY > 22)
            {
                dY = 22;
            }
            else if (dY < -22)
            {
                dY = -22;
            }
        }

        public override void Resize(int W, int H)
        {
            Width = (int) (W / 17)-1;
            Height = (int) (H / 8.5)-1;
            X = ((int)(((double)X / oldsW) * W));
            Y = ((int)(((double)Y / oldsH) * H)); ;
            oldsW = W;
            oldsH = H;
        }


    }
}
