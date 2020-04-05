using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ParallaxStarter
{
    public static class Collisions
    {
        /// <summary>
        /// Detects collisions between this BoundingRectangle and another BoundingRectangle
        /// </summary>
        /// <param name="a">This BoundingRectangle</param>
        /// <param name="b">The other BoundingRectangle</param>
        /// <returns>true if there is a collision, false otherwise</returns>
        public static bool CollidesWith(this BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.X > b.X + b.Width
                 || a.X + a.Width < b.X
                 || a.Y > b.Y + b.Height
                 || a.Y + a.Height < b.Y);
        }

        /// <summary>
        /// Detects if this Vector2 collides with another
        /// </summary>
        /// <param name="v">This Vector2</param>
        /// <param name="other">The other Vector2</param>
        /// <returns>true if there is a collision, false otherwise</returns>
        public static bool CollidesWith(this Vector2 v, Vector2 other)
        {
            return v == other;
        }

        /// <summary>
        /// Detects if this Vector2 collides with a BoundingRectangle
        /// </summary>
        /// <param name="v">This Vector2</param>
        /// <param name="r">The BoundingRectangle</param>
        /// <returns>true if there is a collision, false otherwise</returns>
        public static bool CollidesWith(this Vector2 v, BoundingRectangle r)
        {
            return (r.X <= v.X && v.X <= r.X + r.Width)
                && (r.Y <= v.Y && v.Y <= r.Y + r.Height);
        }

        /// <summary>
        /// Detects if this BoundingRectangle collides with a Vector2
        /// </summary>
        /// <param name="r">This BoundingRectangle</param>
        /// <param name="v">The Vector2</param>
        /// <returns>true if there is a collision, false otherwise</returns>
        public static bool CollidesWith(this BoundingRectangle r, Vector2 v)
        {
            return v.CollidesWith(r);
        }
    }
}
