using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class Banana:MovingEntity 
    {
        public bool hit = false;
        public Banana(int X, int Y, double dX, double dY, int Width, int Height, int sW, int sH) : base(X, Y, dX, dY, Width, Height, sW, sH)
        {
           
        }
        public override void Move()
        {
            
            X += (int)dX;
            Y += (int)dY;
        }
        public override void Resize(int W, int H)
        {
            //Width = (int)(W / 15) - 1;
            //Height = (int)(H / 15) - 1;
            X = ((int)(((double)X / oldsW) * W));
            Y = ((int)(((double)Y / oldsH) * H));
            oldsW = W;
            oldsH = H;
        }
    }
}
