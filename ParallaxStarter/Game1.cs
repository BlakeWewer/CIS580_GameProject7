using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ParallaxStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1296;
            graphics.PreferredBackBufferHeight = 972;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var movesheet = Content.Load<Texture2D>("movesheet");
            var idle_fire_sheet = Content.Load<Texture2D>("idle_fire");
            var deathsheet = Content.Load<Texture2D>("deathsheet");
            var slashsheet = Content.Load<Texture2D>("slash");
            player = new Player(movesheet, deathsheet, idle_fire_sheet, slashsheet);

            var backgroundTexture = Content.Load<Texture2D>("background");
            var backgroundSprite = new StaticSprite(backgroundTexture);
            var backgroundLayer = new ParallaxLayer(this);
            backgroundLayer.Sprites.Add(backgroundSprite);
            backgroundLayer.DrawOrder = 0;
            Components.Add(backgroundLayer);

            var playerLayer = new ParallaxLayer(this);
            playerLayer.Sprites.Add(player);
            playerLayer.DrawOrder = 2;
            Components.Add(playerLayer);

            var midgroundTexture = Content.Load<Texture2D>("road");

            var midgroundSprites = new List<StaticSprite>();
            for (int i = 0; i < 10; i++)
            {
                var position = new Vector2(i * 1296, 663);
                var sprite = new StaticSprite(midgroundTexture, position);
                midgroundSprites.Add(sprite);
            }

            var midgroundLayer = new ParallaxLayer(this);
            midgroundLayer.Sprites.AddRange(midgroundSprites);
            midgroundLayer.DrawOrder = 1;
            //var midgroundScrollController = midgroundLayer.ScrollController as AutoScrollController;
            //midgroundScrollController.Speed = 40f;
            Components.Add(midgroundLayer);

            var foregroundTexture = Content.Load<Texture2D>("small_carsheet");

            var foregroundSprites = new List<StaticSprite>();
            for (int i = 0; i < 10; i++)
            {
                var position = new Vector2(i * 1016, 700);
                var sprite = new StaticSprite(foregroundTexture, position);
                foregroundSprites.Add(sprite);
            }

            var foregroundLayer = new ParallaxLayer(this);
            foreach (var sprite in foregroundSprites)
            {
                foregroundLayer.Sprites.Add(sprite);
            }

            foregroundLayer.DrawOrder = 4;
            //var foregroundScrollController = foregroundLayer.ScrollController as AutoScrollController;
            //foregroundScrollController.Speed = 80f;
            Components.Add(foregroundLayer);

            //var playerScrollController = playerLayer.ScrollController as AutoScrollController;
            //playerScrollController.Speed = 80f;

            backgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.1f);
            midgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.4f);
            playerLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            foregroundLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
