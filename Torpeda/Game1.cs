using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Torpeda
{
    enum Stat
    {
        SplashScreen,
        Game,
        Pause
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Stat Stat = Stat.SplashScreen;
        KeyboardState keyboardState, oldKeyboardState;
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
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.menu = Content.Load<Texture2D>("menu");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashMenu");
            TorGame.backgroundGame = Content.Load<Texture2D>("backgroundGame");
            TorGame.Init(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Ship.Texture2D = Content.Load<Texture2D>("Ship5");
            Scope.Texture2D = Content.Load<Texture2D>("Scope");
            Fire.Texture2D = Content.Load<Texture2D>("Fire");

            // TODO: use this.Content to load your game content here
        }
    
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            switch (Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Space)) Stat = Stat.Game;
                    break;
                case Stat.Game:
                    TorGame.Update();
                    if (keyboardState.IsKeyDown(Keys.Escape)) Stat = Stat.SplashScreen;
                    if (keyboardState.IsKeyDown(Keys.Left)) TorGame.Scope.Left();
                    if (keyboardState.IsKeyDown(Keys.Right)) TorGame.Scope.Right();
                    if (keyboardState.IsKeyDown(Keys.LeftControl) && oldKeyboardState.IsKeyUp(Keys.LeftControl)) TorGame.ShipFire();
                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Delete))
                Exit();
            SplashScreen.Update();
            // TODO: Add your update logic here
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    TorGame.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}