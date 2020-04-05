using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ParallaxStarter
{
    public class Bullet
    {
        Game1 game;
        Texture2D texture;
        public BoundingRectangle Bounds;
        Vector2 Velocity;
        Vector2 Position;
        public TimeSpan active_time;

        const float BULLET_WIDTH = 10;
        const float BULLET_HEIGHT = 5;

        public Bullet(Game1 game, Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.game = game;
            this.texture = texture;
            Position = position;
            Velocity = velocity;
            Bounds = new BoundingRectangle(position.X, position.Y, BULLET_WIDTH, BULLET_HEIGHT);
            active_time = new TimeSpan(0);
        }

        public void Initialize()
        {

        }

        public void LoadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            Position.X += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.X * .5f);
            Position.Y += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.Y * .5f);

            Bounds.X = Position.X;
            Bounds.Y = Position.Y;

            active_time += gameTime.ElapsedGameTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
