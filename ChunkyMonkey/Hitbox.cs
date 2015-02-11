using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyMonkey
{
    class Hitbox
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Hitbox(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public bool Hit(Hitbox check)
        {

            if (((this.X < check.X + check.Width & this.X > check.X) || (this.X + this.Width > check.X & this.X + this.Width < check.X + check.Width)) &
                ((this.Y < check.Y + check.Height & this.Y > check.Y) || (this.Y + this.Height > check.Y & this.Y + this.Height < check.Y + check.Height) || (this.Y + this.Height > check.Y + check.Height & this.Y < check.Y)))
            {
                return true;
            }
            return false;
            /*
            if(this.X >= check.X && this.X <= check.X+check.Width)
            {
                if (this.Y >= check.Y && this.Y <= check.Y + check.Height) 
                {
                    return true;
                }
            }
            else if (this.X <= check.X && this.X >= check.X + check.Width)
            {
                if (this.Y <= check.Y && this.Y >= check.Y + check.Height)
                {
                    return true;
                }
            }
            return false;
             */
        }
    }
}
