using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;

namespace GameCode.GameObjects;

public class Tetris
{
    public int[,] Board { get; set; } = new int[10, 20];
    public int CurrentPieceType { get; set; } = 0;
    public int NextPieceType { get; set; } = 0;
    public int CurrentPieceRotation { get; set; } = 0;
    public int CurrentPieceX { get; set; } = 3;
    public int CurrentPieceY { get; set; } = 0;

    public int NextPieceX { get; set; } = 3;
    public int NextPieceY { get; set; } = 0;

    //Time = (0.8-((Level-1)*0.007))(Level-1)
    public float StepSpeed { get; set; } = 1f;
    public float MoveSpeed { get; set; } = 0.1f;

    private float moveTimer = 0f;
    private float stepTimer = 0f;
    private int xOffset;

    private Texture2D tileTexture;

    public List<List<int[,]>> Pieces { get; set; } = new List<List<int[,]>>()
    {
        //I
        new()
        {
            new int[4,4]
            {
                {0,0,0,0},
                {1,1,1,1},
                {0,0,0,0},
                {0,0,0,0},
            },
            new int[4,4]
            {
                {0,0,1,0},
                {0,0,1,0},
                {0,0,1,0},
                {0,0,1,0},
            },
            new int[4,4]
            {
                {0,0,0,0},
                {0,0,0,0},
                {1,1,1,1},
                {0,0,0,0},
            },
            new int[4,4]
            {
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0},
            },
        },

        //J
        new()
        {
            new int[3,3]
            {
                {1,0,0},
                {1,1,1},
                {0,0,0},
            },
            new int[3,3]
            {
                {0,1,1},
                {0,1,0},
                {0,1,0},
            },
            new int[3,3]
            {
                {0,0,0},
                {1,1,1},
                {0,0,1},
            },
            new int[3,3]
            {
                {0,1,0},
                {0,1,0},
                {1,1,0},
            },
        },

        //L
        new()
        {
            new int[3,3]
            {
                {0,0,1},
                {1,1,1},
                {0,0,0},
            },
            new int[3,3]
            {
                {0,1,0},
                {0,1,0},
                {0,1,1},
            },
            new int[3,3]
            {
                {0,0,0},
                {1,1,1},
                {1,0,0},
            },
            new int[3,3]
            {
                {1,1,0},
                {0,1,0},
                {0,1,0},
            },
        },

        //O
        new()
        {
            new int[4,4]
            {
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0},
            },
            new int[4,4]
            {
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0},
            },
            new int[4,4]
            {
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0},
            },
            new int[4,4]
            {
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0},
            },
        },

        //S
        new()
        {
            new int[3,3]
            {
                {0,1,1},
                {1,1,0},
                {0,0,0},
            },
            new int[3,3]
            {
                {0,1,0},
                {0,1,1},
                {0,0,1},
            },
            new int[3,3]
            {
                {0,0,0},
                {0,1,1},
                {1,1,0},
            },
            new int[3,3]
            {
                {1,0,0},
                {1,1,0},
                {0,1,0},
            },
        },

        //T
        new()
        {
            new int[3,3]
            {
                {0,1,0},
                {1,1,1},
                {0,0,0},
            },
            new int[3,3]
            {
                {0,1,0},
                {0,1,1},
                {0,1,0},
            },
            new int[3,3]
            {
                {0,0,0},
                {1,1,1},
                {0,1,0},
            },
            new int[3,3]
            {
                {0,1,0},
                {1,1,0},
                {0,1,0},
            },
        },

        //Z
        new()
        {
            new int[3,3]
            {
                {1,1,0},
                {0,1,1},
                {0,0,0},
            },
            new int[3,3]
            {
                {0,0,1},
                {0,1,1},
                {0,1,0},
            },
            new int[3,3]
            {
                {0,0,0},
                {1,1,0},
                {0,1,1},
            },
            new int[3,3]
            {
                {0,1,0},
                {1,1,0},
                {1,0,0},
            },
        },

  
    };


    public Tetris(BaseGame game, int xoffset)
    {
        xOffset = xoffset;
        CurrentPieceType = GetRandomPiece();
        NextPieceType = GetRandomPiece();

        tileTexture = game.Content.Load<Texture2D>(@"sprites\tile");
    }

    public void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState)
    {

        if (keyState.WasKeyJustDown(Keys.Left))
        {
            CurrentPieceX--;
        }
        if (keyState.WasKeyJustDown(Keys.Right))
        {
            CurrentPieceX++;
        }
        if (keyState.WasKeyJustDown(Keys.Down))
        {
            CurrentPieceY++;
        }
        if (keyState.WasKeyJustDown(Keys.Up))
        {
            CurrentPieceRotation++;
            if (CurrentPieceRotation > 3)
                CurrentPieceRotation = 0;
        }

        moveTimer += dt;
        stepTimer += dt;

        if (moveTimer >= MoveSpeed)
        {
            moveTimer = 0f;
            if (keyState.IsKeyDown(Keys.Left))
            {
                CurrentPieceX--;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                CurrentPieceX++;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                CurrentPieceY++;
            }
        }

        if (stepTimer >= StepSpeed)
        {
            stepTimer = 0f;
            CurrentPieceY++;
        }

        if (CurrentPieceX < 1)
            CurrentPieceX = 1;

        if (CurrentPieceX > 8)
            CurrentPieceX = 8;

        if (CurrentPieceY > 19)
            CurrentPieceY = 19;

        if (CurrentPieceY < 0)
            CurrentPieceY = 0;

    }

    public void Draw(SpriteBatch sb)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                if (Board[x, y] == 1)
                {
                    sb.Draw(tileTexture, new Rectangle(new Point((x * 32) + xOffset, y * 32), new Point(32, 32)), Color.White);
                }
            }
        }

        var currentPiece = Pieces[CurrentPieceType][CurrentPieceRotation];
        var currentPieceColor = GetBlockColor();
        for (int x = 0; x < currentPiece.GetLength(0); x++)
        {
            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                if (currentPiece[y, x] == 1)
                    sb.Draw(tileTexture, new Rectangle(new Point(((x + CurrentPieceX) * 32) + xOffset, (y + CurrentPieceY) * 32), new Point(32, 32)), currentPieceColor);
            }
        }
    }

    public Color GetBlockColor()
    {
        if (CurrentPieceType == 0)
            return Color.Cyan;
        if (CurrentPieceType == 1)
            return Color.Blue;
        if (CurrentPieceType == 2)
            return Color.Orange;
        if (CurrentPieceType == 3)
            return Color.Yellow;
        if (CurrentPieceType == 4)
            return Color.Green;
        if (CurrentPieceType == 5)
            return Color.Purple;
        if (CurrentPieceType == 6)
            return Color.Red;

        return Color.White;
    }
    public int GetRandomPiece()
    {
        var random = new Random();
        var choice = random.Next(0, 7);

        return choice;
    }

}