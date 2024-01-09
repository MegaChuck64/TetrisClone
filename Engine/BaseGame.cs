using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine
{
    public abstract class BaseGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public BaseGame(int width = 800, int height = 600)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ScreenWidth = width;
            ScreenHeight = height;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Load();
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Update(dt);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Draw(dt);


            base.Draw(gameTime);
        }

        public abstract void Load();
        public abstract void Draw(float dt);
        public abstract void Update(float dt);
    }
}
