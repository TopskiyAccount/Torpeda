using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        int ammoCount = 3;
        int maxAmmoCount = 3;
        int reloadsRemaining = 10;
        bool isReloading = false;
        float timeSinceLastShot = 0f;
        const float reloadTime = 5f;

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
            TorGame.Font = Content.Load<SpriteFont>("font");

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
                    if (keyboardState.IsKeyDown(Keys.LeftControl) && oldKeyboardState.IsKeyUp(Keys.LeftControl))
                        if (!isReloading && ammoCount > 0)
                        {
                            Shoot();
                        }
                    if (keyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R))
                    {
                        if (!isReloading && reloadsRemaining > 0 && ammoCount < maxAmmoCount)
                        {
                            isReloading = true;
                        }
                    }
                    if (isReloading)
                    {
                        timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (timeSinceLastShot >= reloadTime)
                        {
                            timeSinceLastShot = 0f;
                            isReloading = false;
                            ammoCount = maxAmmoCount;
                            reloadsRemaining--;
                        }
                    }

                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Delete))
                Exit();
            SplashScreen.Update();
            TorGame.Update();
            // TODO: Add your update logic here
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }
        void Shoot()
        {
            // Your shoot logic here
            TorGame.ShipFire();
            ammoCount--;
            timeSinceLastShot = 0f;
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
            _spriteBatch.DrawString(Content.Load<SpriteFont>("font"), $"Ammo: {ammoCount}/{maxAmmoCount}", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("font"), $"Reloads remaining: {reloadsRemaining}", new Vector2(10, 60), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("font"), $"Reloading: {isReloading}", new Vector2(10, 100), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}