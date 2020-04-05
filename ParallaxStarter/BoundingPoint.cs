using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ParallaxStarter
{
    public struct BoundingPoint
    {
        public float X;

        public float Y;


        public BoundingPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public BoundingPoint(Vector2 position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }

        /// <summary>
        /// Cast operator for casting into a Rectangle
        /// </summary>
        /// <param name="br"></param>
        public static implicit operator Rectangle(BoundingPoint br)
        {
            return new Rectangle(
                (int)br.X,
                (int)br.Y,
                (int)1,
                (int)1);
        }
    }
}
