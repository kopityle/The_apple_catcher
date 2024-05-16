using AppleCatcher;
using Microsoft.Xna.Framework;
using Microsoft .Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_apple_catcher

{
    public enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Stat Stat= Stat.SplashScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1020;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");
            Apples.Init(_spriteBatch, 1920, 1020);
            Sheets.Texture2D = Content.Load<Texture2D>("sheet");
            Basket.Texture2D = Content.Load<Texture2D>("basket");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            switch (Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Space)) Stat = Stat.Game;
                        break;
                case Stat.Game:
                    Apples.Update();
                    if (keyboardState.IsKeyDown(Keys.Escape)) Stat = Stat.SplashScreen;
                    if (keyboardState.IsKeyDown(Keys.Left)) Apples.Basket.Left();
                    if (keyboardState.IsKeyDown(Keys.Right)) Apples.Basket.Right();
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            SplashScreen.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (Stat)
            { 
            case Stat.SplashScreen:
                    SplashScreen.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    Apples.Draw();
                    break;
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}