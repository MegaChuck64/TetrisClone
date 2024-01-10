using Engine;
using GameCode.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using System.Collections.Generic;

namespace GameCode.Scenes;

public class GameScene : Scene
{
    //12x22
    private readonly List<Tile> bordertiles = new();
    private SpriteBatch sb;
    private Texture2D tileTexture;
    private int offsetX = 6 * 32;
    private Tetris tetris;
    public GameScene(BaseGame game) : base(game)
    {
        for (int x = 0; x < 12; x++)
        {
            bordertiles.Add(new Tile(new Point(x, 0), new Point(32, 32)));
            bordertiles.Add(new Tile(new Point(x, 21), new Point(32, 32)));
        }
        for (int y = 1; y < 21; y++)
        {
            bordertiles.Add(new Tile(new Point(0, y), new Point(32, 32)));
            bordertiles.Add(new Tile(new Point(11, y), new Point(32, 32)));
        }
    }

    public override void Load()
    {
        sb = new SpriteBatch(Game.GraphicsDevice);
        tileTexture = Game.Content.Load<Texture2D>(@"sprites\tile");
        tetris = new Tetris(Game);
    }

    public override void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState)
    {
        tetris.Update(dt, keyState, mouseState);
    }

    public override void Draw(float dt)
    {
        var borderTint = new Color(0.8f, 0.8f, 0.8f, 1f);
        sb.Begin();
        foreach (var tile in bordertiles)
        {
            tile.Draw(sb, tileTexture, offsetX, borderTint);
        }
        tetris.Draw(sb);
        sb.End();
    }
}

public class Tile
{
    public Point TilePosition { get; set; }
    public Point TileSize { get; set; }
    public Point ScreenPosition => new(TilePosition.X * TileSize.X, TilePosition.Y * TileSize.Y);

    public Tile(Point tilePosition, Point tileSize)
    {
        TilePosition = tilePosition;
        TileSize = tileSize;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture, int xOffset, Color tint)
    {
        spriteBatch.Draw(texture, new Rectangle(ScreenPosition + new Point(xOffset, 0), TileSize), tint);
    }

}