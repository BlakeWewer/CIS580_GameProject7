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
    public class Enemy
    {
        public Game1 game;
        public Texture2D texture;
        TimeSpan timer;
        int frame;
        public BoundingRectangle Bounds;
        public Vector2 curPosition;
        public Vector2 Velocity;
        Vector2 playerPos;
        public bool destroyed;
        SpriteFont spriteFont;

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        const float PLAYER_SPEED = 100;

        /// <summary>
        /// The angle the helicopter should tilt
        /// </summary>
        float angle = 0;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int MOVING_FRAME_WIDTH = 75;

        /// <summary>
        /// The height of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 92;

        const int MOVING_FRAME_WIDTH_GAP = 1;

        public Enemy(Game1 game, Vector2 position)
        {
            this.game = game;
            timer = new TimeSpan(0);
            Bounds = new BoundingRectangle(position, MOVING_FRAME_WIDTH, FRAME_HEIGHT);
            texture = game.Content.Load<Texture2D>("movesheet");
            spriteFont = game.Content.Load<SpriteFont>("font_score");
            curPosition = position;
            Velocity = new Vector2(-1, 0);
            frame = 0;
        }


        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            playerPos = playerPosition;
            // Add movement towards player & animation
            var delta = gameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 dirToPlayer = playerPosition - (Position() + new Vector2(33, 84));
            Velocity = dirToPlayer;
            Velocity.Normalize();
            curPosition += (float)delta * PLAYER_SPEED * Velocity;

            if (curPosition.Y < 675)
            {
                var deltaP = 675 - curPosition.Y;
                curPosition += new Vector2(0, (float)(deltaP));
            }
            if (curPosition.Y > 972 - FRAME_HEIGHT)
            {
                var deltaP = 972 - FRAME_HEIGHT - curPosition.Y;
                curPosition += new Vector2(0, (float)(deltaP));
            }
            Bounds.X = curPosition.X;
            Bounds.Y = curPosition.Y;


            timer += gameTime.ElapsedGameTime;
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;

                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }
            frame %= 6;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(
                frame * (MOVING_FRAME_WIDTH + MOVING_FRAME_WIDTH_GAP), // X value 
                0, // Y value
                MOVING_FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );

            SpriteEffects spriteEffect = SpriteEffects.FlipHorizontally;

            var enemyColor = Color.Red;

            spriteBatch.Draw(texture, new Vector2((int) curPosition.X, (int)curPosition.Y), source, enemyColor, angle, Vector2.Zero, 1f, spriteEffect, 0f);
        }

        public Vector2 Position()
        {
            return new Vector2(Bounds.X, Bounds.Y);
        }
    }
}
