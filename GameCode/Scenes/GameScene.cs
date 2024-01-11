using Engine;
using GameCode.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

namespace GameCode.Scenes;

public class GameScene : Scene
{
    //12x22
    private SpriteBatch sb;
    private Texture2D tileTexture;
    private int offsetX = 6 * 32;
    private Tetro tetro;
    public GameScene(BaseGame game) : base(game)
    {

    }

    public override void Load()
    {
        sb = new SpriteBatch(Game.GraphicsDevice);
        tileTexture = Game.Content.Load<Texture2D>(@"sprites\tile");
        tetro = new Tetro();
    }

    public override void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState)
    {
        tetro.Update(dt, keyState);
    }

    public override void Draw(float dt)
    {
        sb.Begin();
        tetro.Draw(sb, tileTexture, offsetX);
        sb.End();
    }
}
