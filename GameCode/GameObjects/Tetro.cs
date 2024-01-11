using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;

namespace GameCode.GameObjects;

public class Tetro
{

    public BlockType[,] Board { get; set; } = new BlockType[12, 22];

    public BlockType CurrentType { get; set; }
    public Rotation CurrentRotation { get; set; }
    public Point CurrentPosition { get; set; }

    public Tetro()
    {
        CurrentType = GetRandomBlockType();
        CurrentRotation = Rotation.Default;
        CurrentPosition = new Point(5, 0);

        for (int y = 0; y < Board.GetLength(1); y++)
        {
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                if (x == 0 || x == Board.GetLength(0) - 1 || y == Board.GetLength(1) - 1)
                {
                    Board[x, y] = BlockType.Wall;
                }
                else
                {
                    Board[x, y] = BlockType.None;
                }
            }
        }


    }

    private float stepTimer = 0f;
    private float stepSpeed = 0.8f;
    private float movetimer = 0f;
    private float movespeed = 0.1f;
    public void Update(float dt, KeyboardStateExtended keyState)
    {

        stepTimer += dt;
        if (stepTimer >= stepSpeed)
        {
            stepTimer = 0f;

            if (CanMove(new Point(CurrentPosition.X, CurrentPosition.Y + 1), CurrentRotation))
            {
                CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
            }
            else
            {
                var block = GetBlock(CurrentType, CurrentRotation);
                for (int y = 0; y < block.GetLength(1); y++)
                {
                    for (int x = 0; x < block.GetLength(0); x++)
                    {
                        if (block[x, y] == 1)
                        {
                            var boardX = CurrentPosition.X + x;
                            var boardY = CurrentPosition.Y + y;
                            if (Board[boardX, boardY] == BlockType.None)
                                Board[boardX, boardY] = CurrentType;
                            else if (boardY == 0)
                                throw new Exception("Game Over");
                        }
                    }
                }

                CurrentType = GetRandomBlockType();
                CurrentRotation = Rotation.Default;
                CurrentPosition = new Point(5, 0);
            }
        }
        if (keyState.WasKeyJustDown(Keys.Up))
        {
            var nextRotation = (Rotation)(((int)CurrentRotation + 1) % 4);
            if (CanMove(CurrentPosition, nextRotation))
            {
                CurrentRotation = nextRotation;
            }
        }
        movetimer += dt;
        if (movetimer >= movespeed)
        {
            movetimer = 0f;

            if (keyState.IsKeyDown(Keys.Left))
            {
                if (CanMove(new Point(CurrentPosition.X - 1, CurrentPosition.Y), CurrentRotation))
                {
                    CurrentPosition = new Point(CurrentPosition.X - 1, CurrentPosition.Y);
                }
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                if (CanMove(new Point(CurrentPosition.X + 1, CurrentPosition.Y), CurrentRotation))
                {
                    CurrentPosition = new Point(CurrentPosition.X + 1, CurrentPosition.Y);
                }
            }   
            else if (keyState.IsKeyDown(Keys.Down))
            {
                if (CanMove(new Point(CurrentPosition.X, CurrentPosition.Y + 1), CurrentRotation))
                {
                    CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
                }
            }            
        }
    }

    public void Draw(SpriteBatch sb, Texture2D tileTexture, int offsetX)
    {


        for (int y = 0; y < Board.GetLength(1); y++)
        {
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                var screenX = x * 32 + offsetX;
                var screenY = y * 32;
                sb.Draw(tileTexture, new Rectangle(screenX, screenY, 32, 32), GetBlockColor(Board[x, y]));
            }
        }

        var block = GetBlock(CurrentType, CurrentRotation);
        for (int y = 0; y < block.GetLength(1); y++)
        {
            for (int x = 0; x < block.GetLength(0); x++)
            {
                if (block[x, y] == 1)
                {
                    var boardX = CurrentPosition.X + x;
                    var boardY = CurrentPosition.Y + y;
                    var screenX = boardX * 32 + offsetX;
                    var screenY = boardY * 32;
                    sb.Draw(tileTexture, new Rectangle(screenX, screenY, 32, 32), GetBlockColor(CurrentType));
                }
            }
        }
    }

    public bool CanMove(Point position, Rotation rotation)
    {
        var block = GetBlock(CurrentType, rotation);

        //if (position.X < 0 ||
        //    position.X + block.GetLength(0) > Board.GetLength(0) ||
        //    position.Y < 0 ||
        //    position.Y + block.GetLength(1) > Board.GetLength(1))
        //{
        //    return false;
        //}

        for (int y = 0; y < block.GetLength(1); y++)
        {
            for (int x = 0; x < block.GetLength(0); x++)
            {
                if (block[x, y] == 1)
                {
                    var boardX = position.X + x;
                    var boardY = position.Y + y;
                    if (boardX < 0 || boardX >= Board.GetLength(0) || boardY < 0 || boardY >= Board.GetLength(1))
                    {
                        return false;
                    }
                    if (Board[boardX, boardY] != BlockType.None)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public int[,] GetBlock(BlockType type, Rotation rotation)
    {
        var block = new int[4, 4];
        switch (type)
        {
            case BlockType.I:
                block = GetIBlock(rotation);
                break;
            case BlockType.O:
                block = GetOBlock(rotation);
                break;
            case BlockType.T:
                block = GetTBlock(rotation);
                break;
            case BlockType.S:
                block = GetSBlock(rotation);
                break;
            case BlockType.Z:
                block = GetZBlock(rotation);
                break;
            case BlockType.J:
                block = GetJBlock(rotation);
                break;
            case BlockType.L:
                block = GetLBlock(rotation);
                break;
        }

        return block;
    }

    public int[,] GetIBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
            case Rotation.Inverted:
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[1, 2] = 1;
                block[1, 3] = 1;
                break;
            case Rotation.Right:
            case Rotation.Left:
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[3, 1] = 1;
                break;
        }
        return block;
    }

    public int[,] GetOBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        block[1, 1] = 1;
        block[1, 2] = 1;
        block[2, 1] = 1;
        block[2, 2] = 1;
        return block;
    }

    public int[,] GetTBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
                block[1, 0] = 1;
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                break;
            case Rotation.Right:
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[1, 2] = 1;
                break;
            case Rotation.Inverted:
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[1, 2] = 1;
                break;
            case Rotation.Left:
                block[1, 0] = 1;
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[1, 2] = 1;
                break;
        }
        return block;
    }

    public int[,] GetSBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
            case Rotation.Inverted:
                block[1, 0] = 1;
                block[2, 0] = 1;
                block[0, 1] = 1;
                block[1, 1] = 1;
                break;
            case Rotation.Right:
            case Rotation.Left:
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[2, 2] = 1;
                break;
        }
        return block;
    }

    public int[,] GetZBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
            case Rotation.Inverted:
                block[0, 0] = 1;
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                break;
            case Rotation.Right:
            case Rotation.Left:
                block[2, 0] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[1, 2] = 1;
                break;
        }
        return block;
    }

    public int[,] GetJBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
                block[0, 0] = 1;
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                break;
            case Rotation.Right:
                block[1, 0] = 1;
                block[2, 0] = 1;
                block[1, 1] = 1;
                block[1, 2] = 1;
                break;
            case Rotation.Inverted:
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[2, 2] = 1;
                break;
            case Rotation.Left:
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[0, 2] = 1;
                block[1, 2] = 1;
                break;
        }
        return block;
    }

    public int[,] GetLBlock(Rotation rotation)
    {
        var block = new int[4, 4];
        switch (rotation)
        {
            case Rotation.Default:
                block[2, 0] = 1;
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                break;
            case Rotation.Right:
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[1, 2] = 1;
                block[2, 2] = 1;
                break;
            case Rotation.Inverted:
                block[0, 1] = 1;
                block[1, 1] = 1;
                block[2, 1] = 1;
                block[0, 2] = 1;
                break;
            case Rotation.Left:
                block[0, 0] = 1;
                block[1, 0] = 1;
                block[1, 1] = 1;
                block[1, 2] = 1;
                break;
        }
        return block;
    }



    public enum BlockType
    {
        None,
        Wall,
        I,
        O,
        T,
        S,
        Z,
        J,
        L
    }

    public enum Rotation
    {
        Default,
        Right,
        Inverted,
        Left
    }

    public Color GetBlockColor(BlockType blockType)
    {
        return blockType switch
        {
            BlockType.I => Color.Cyan,
            BlockType.O => Color.Yellow,
            BlockType.T => Color.Purple,
            BlockType.S => Color.Green,
            BlockType.Z => Color.Red,
            BlockType.J => Color.Blue,
            BlockType.L => Color.Orange,
            BlockType.Wall => Color.Gray,
            BlockType.None => Color.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(blockType), blockType, null)
        };
    }



    public BlockType GetRandomBlockType()
    {
        var values = Enum.GetValues(typeof(BlockType));
        var random = new Random();
        return (BlockType)values.GetValue(random.Next(2,values.Length));
    }


}