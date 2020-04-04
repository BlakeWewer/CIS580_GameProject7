using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace ParallaxStarter
{
    public class Player : ISprite
    {
        /// <summary>
        /// A spritesheet containing the movement images
        /// </summary>
        public Texture2D movesheet;

        /// <summary>
        /// A spritesheet containing the idle_fire images
        /// </summary>
        Texture2D idle_fire_sheet;

        /// <summary>
        /// A spritesheet containing the slash images
        /// </summary>
        Texture2D slashsheet;

        /// <summary>
        /// A spritesheet containing the death images
        /// </summary>
        Texture2D deathsheet;

        public List<Texture2D> spriteList = new List<Texture2D>();

        /// <summary>
        /// The portion of the spritesheet that is the helicopter
        /// </summary>
        Rectangle sourceRect = new Rectangle
        {
            X = 0,
            Y = 0,
            Width = 66,
            Height = 84
        };

        enum MovingDirection
        {
            Right = 0,
            Left = 1,
            Up = 2,
            Down = 3,
            Idle = 4,
        }

        enum LivingState
        {
            Alive = 0,
            Dead = 1,
        }

        enum FightingState
        {
            Idle = 0,
            Shoot = 1,
            Slash = 2,
        }

        MovingDirection moveDir;
        MovingDirection prev_moveDir;
        LivingState liveState;
        FightingState fightState;

        TimeSpan timer;
        int frame;
        public BoundingRectangle Bounds;
        TimeSpan fireTimer;
        int fireFrame;
        double fireDelay = 1000;
        double fireRate = 372;
        TimeSpan fireAnimationTimer;
        TimeSpan slashTimer;
        int slashFrame;
        double slashDelay = 1000;
        double slashRate = 496;
        TimeSpan slashAnimationTimer;

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        const float PLAYER_SPEED = 100;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int MOVING_FRAME_WIDTH = 75;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int IDLE_FIRE_FRAME_WIDTH = 77;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int SLASH_FRAME_WIDTH = 80;

        /// <summary>
        /// The height of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 92;

        const int MOVING_FRAME_WIDTH_GAP = 1;
        const int IDLE_FIRE_FRAME_WIDTH_GAP = 0;
        const int SLASH_FRAME_WIDTH_GAP = 0;

        /// <summary>
        /// The origin of the move sprite
        /// </summary>
        Vector2 move_origin = new Vector2(33, 84);

        /// <summary>
        /// The player's position in the world
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The angle the helicopter should tilt
        /// </summary>
        float angle = 0;

        /// <summary>
        /// How fast the player moves
        /// </summary>
        public float Speed { get; set; } = 100;

        /// <summary>
        /// Constructs a player
        /// </summary>
        /// <param name="movesheet">The player's movesheet</param>
        public Player(Texture2D movesheet, Texture2D deathsheet, Texture2D idle_fire_sheet, Texture2D slashsheet)
        {
            this.movesheet = movesheet;
            this.deathsheet = deathsheet;
            this.idle_fire_sheet = idle_fire_sheet;
            this.slashsheet = slashsheet;
            this.Position = new Vector2(200, 700);

            spriteList.Add(movesheet);
            spriteList.Add(deathsheet);
            spriteList.Add(idle_fire_sheet);
            spriteList.Add(slashsheet);

            moveDir = MovingDirection.Idle;
            prev_moveDir = MovingDirection.Idle;
            liveState = LivingState.Alive;
            fightState = FightingState.Idle;

            timer = new TimeSpan(0);
            fireAnimationTimer = new TimeSpan(0);
            slashAnimationTimer = new TimeSpan(0);
            Bounds = new BoundingRectangle(Position.X, Position.Y, 66, 84);
            fireTimer = new TimeSpan(0);
            slashTimer = new TimeSpan(0);
        }

        /// <summary>
        /// Updates the player position based on GamePad or Keyboard input
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            if(liveState == LivingState.Alive)
            {
                Vector2 direction = Vector2.Zero;

                // Use GamePad for input
                var gamePad = GamePad.GetState(0);

                // The thumbstick value is a vector2 with X & Y between [-1f and 1f] and 0 if no GamePad is available
                direction.X = gamePad.ThumbSticks.Left.X;

                // We need to inverty the Y axis
                direction.Y = -gamePad.ThumbSticks.Left.Y;

                // Override with keyboard input
                bool idle = true;
                var keyboard = Keyboard.GetState();
                var speedVar = 1;
                if (keyboard.IsKeyDown(Keys.Space)) speedVar = 20;

                if(keyboard.IsKeyDown(Keys.O) || keyboard.IsKeyDown(Keys.Z))
                {
                    if(fireTimer.TotalMilliseconds > fireDelay)
                    {
                        fightState = FightingState.Shoot;
                        fireTimer = new TimeSpan(0);

                        // Create Bullet
                    }
                }
                fireTimer += gameTime.ElapsedGameTime;

                if (keyboard.IsKeyDown(Keys.P) || keyboard.IsKeyDown(Keys.X))
                {
                    if (slashTimer.TotalMilliseconds > slashDelay)
                    {
                        fightState = FightingState.Slash;
                        slashTimer = new TimeSpan(0);

                        // Create Knife HitBox
                    }
                }
                slashTimer += gameTime.ElapsedGameTime;

                if(fightState != FightingState.Idle)
                {
                    if (slashTimer.TotalMilliseconds > slashRate && fireTimer.TotalMilliseconds > fireRate)
                    {
                        fightState = FightingState.Idle;
                    }
                }

                if(fightState == FightingState.Idle)
                {
                    if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                    {
                        prev_moveDir = moveDir;
                        idle = false;
                        moveDir = MovingDirection.Left;
                        direction.X -= 1 * speedVar;
                    }
                    if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                    {
                        prev_moveDir = moveDir;
                        idle = false;
                        moveDir = MovingDirection.Right;
                        direction.X += 1 * speedVar;
                    }
                    if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    {
                        idle = false;
                        moveDir = MovingDirection.Up;
                        direction.Y -= 1;
                    }
                    if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    {
                        idle = false;
                        moveDir = MovingDirection.Down;
                        direction.Y += 1;
                    }
                    if (idle)
                    {
                        moveDir = MovingDirection.Idle;
                    }
                }
                

                // Move the player
                Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * direction;

                // Check boundaries
                if (Position.Y < 675)
                {
                    var delta = 675 - Position.Y;
                    Position += new Vector2(0, (float)(delta));
                }
                if (Position.Y > 972)
                {
                    var delta = 972 - Position.Y;
                    Position += new Vector2(0, (float)(delta));
                }
                if (Position.X < 200)
                {
                    var delta = 200 - Position.X;
                    Position += new Vector2((float)(delta), 0);
                }

                // Update the player animation timer when the player is moving
                if (moveDir != MovingDirection.Idle) timer += gameTime.ElapsedGameTime;
                if (fightState != FightingState.Idle)
                {
                    fireAnimationTimer += gameTime.ElapsedGameTime;
                    slashAnimationTimer += gameTime.ElapsedGameTime;
                }

                // Determine the frame should increase.  Using a while 
                // loop will accomodate the possiblity the animation should 
                // advance more than one frame.
                if (fightState == FightingState.Idle)
                {
                    while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                    {
                        // increase by one frame
                        frame++;

                        // reduce the timer by one frame duration
                        timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }
                

                if(fightState == FightingState.Shoot)
                {
                    while (fireAnimationTimer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                    {
                        // increase by one frame
                        fireFrame++;

                        // reduce the timer by one frame duration
                        fireAnimationTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }

                if (fightState == FightingState.Slash)
                {
                    while (slashAnimationTimer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                    {
                        // increase by one frame
                        slashFrame++;

                        // reduce the timer by one frame duration
                        slashAnimationTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }

                // Keep the frame within Bounds (there are four frames)
                frame %= 6;
                fireFrame %= 3;
                slashFrame %= 4;

                Bounds.X = Position.X;
                Bounds.Y = Position.Y;
            }
            
        }

        /// <summary>
        /// Draws the player sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(liveState == LivingState.Alive)
            {
                Rectangle source = new Rectangle(
                frame * (MOVING_FRAME_WIDTH + MOVING_FRAME_WIDTH_GAP), // X value 
                0, // Y value
                MOVING_FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );

                if (moveDir == MovingDirection.Idle)
                {
                    source = new Rectangle(
                    0, // X value 
                    0, // Y value
                    IDLE_FIRE_FRAME_WIDTH, // Width 
                    FRAME_HEIGHT // Height
                    );
                }
                if (fightState == FightingState.Shoot)
                {
                    source = new Rectangle(
                    fireFrame * (IDLE_FIRE_FRAME_WIDTH + IDLE_FIRE_FRAME_WIDTH_GAP), // X value 
                    0, // Y value
                    IDLE_FIRE_FRAME_WIDTH, // Width 
                    FRAME_HEIGHT // Height
                    );
                }
                if (fightState == FightingState.Slash)
                {
                    source = new Rectangle(
                    fireFrame * (SLASH_FRAME_WIDTH + SLASH_FRAME_WIDTH_GAP), // X value 
                    0, // Y value
                    SLASH_FRAME_WIDTH, // Width 
                    FRAME_HEIGHT // Height
                    );
                }


                SpriteEffects spriteEffect = SpriteEffects.None;
                
                if (prev_moveDir == MovingDirection.Left)
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }
                // Render the player
                if(fightState == FightingState.Idle)
                {
                    if (moveDir != MovingDirection.Idle)
                        spriteBatch.Draw(movesheet, Position, source, Color.White, angle, move_origin, 1f, spriteEffect, 0.7f);
                    else
                        spriteBatch.Draw(idle_fire_sheet, Position, source, Color.White, angle, move_origin, 1f, spriteEffect, 0.7f);
                }
                else if(fightState == FightingState.Shoot)
                {
                    spriteBatch.Draw(idle_fire_sheet, Position, source, Color.White, angle, move_origin, 1f, spriteEffect, 0.7f);
                }
                else if(fightState == FightingState.Slash)
                {
                    spriteBatch.Draw(slashsheet, Position, source, Color.White, angle, move_origin, 1f, spriteEffect, 0.7f);
                }
                
            }

        }
    }
}
