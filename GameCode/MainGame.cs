using Engine;
using GameCode.Scenes;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace GameCode;

public class MainGame : BaseGame
{
    //12x22 by 32x32
    //so we want the height to be 22 * 32 = 704
    //and the width to be 12 * 32 * 2 = 768
    private GameScene gameScene;
    public MainGame() : base(768, 704)
    {
        BackgroundColor = Color.Black;
    }

    public override void Load()
    {
        gameScene = new GameScene(this);
        gameScene.Load();
    }

    public override void Update(float dt)
    {
        var keyState = KeyboardExtended.GetState();
        var mouseState = MouseExtended.GetState();

        gameScene.Update(dt, keyState, mouseState);
    }

    public override void Draw(float dt)
    {
        gameScene.Draw(dt);
    }

}