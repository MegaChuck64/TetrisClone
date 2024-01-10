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
    public int[,] Board { get; set; } = new int[12, 22];
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


        //init board
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (x == 0 || x == Board.GetLength(0) - 1 || y == Board.GetLength(1) - 1 || y == 0)
                    Board[x, y] = 1;
            }
        }

    }

    public void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState)
    {
        var nextX = CurrentPieceX;
        var nextY = CurrentPieceY;
        var nextRotation = CurrentPieceRotation;

        if (keyState.WasKeyJustDown(Keys.Left))
        {
            nextX--;
        }
        if (keyState.WasKeyJustDown(Keys.Right))
        {
            nextX++;
        }
        if (keyState.WasKeyJustDown(Keys.Down))
        {
            nextY++;
        }
        if (keyState.WasKeyJustDown(Keys.Up))
        {
            nextRotation++;
            if (nextRotation > 3)
                nextRotation = 0;
        }

        moveTimer += dt;
        stepTimer += dt;

        if (moveTimer >= MoveSpeed)
        {
            moveTimer = 0f;
            if (keyState.IsKeyDown(Keys.Left))
            {
                nextX--;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                nextX++;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                nextY++;
            }
        }

        if (stepTimer >= StepSpeed)
        {
            stepTimer = 0f;
            nextY++;
        }


        var (x, y, rot) = GetNextState(nextX, nextY, nextRotation);
        CurrentPieceX = x;
        CurrentPieceY = y;
        CurrentPieceRotation = rot;
    }

    public (int x, int y, int rot) GetNextState(int nextX, int nextY, int nextRotation)
    {
        var currentPiece = Pieces[CurrentPieceType][nextRotation];
        var result = (x: nextX, y: nextY, rot: nextRotation);

        //check if in bounds
        var rect = GetSmallestRect(nextX, nextY, currentPiece);
        if (rect.X < 1)
            result.x = 1;
        if (rect.X + rect.Width > Board.GetLength(0) - 2)
            result.x = Board.GetLength(0) - 2 - rect.Width;
        if (rect.Y + rect.Height > Board.GetLength(1) - 2)
            result.y = Board.GetLength(1) - 2 - rect.Height;
        if (rect.Y < 1)
            result.y = 1;

        //check if can move
        if (!CanMoveTo(result.x, result.y, currentPiece, Board))
        {
            result.x = CurrentPieceX;
            result.y = CurrentPieceY;
        }

        return result;
    }

    public bool CanMoveTo(int x, int y, int[,] piece, int[,] board)
    {
        var result = true;

        for (int i = 0; i < piece.GetLength(0); i++)
        {
            for (int j = 0; j < piece.GetLength(1); j++)
            {
                if (piece[i, j] == 1)
                {
                    if (board[x + i, y + j] == 1)
                        result = false;
                }
            }
        }

        return result;
    }

    public Rectangle GetSmallestRect(int x, int y, int[,] piece)
    {
        var rect = new Rectangle(x, y, 0, 0);

        var width = 0;
        var height = 0;
        for (int i = 0; i < piece.GetLength(0); i++)
        {
            for (int j = 0; j < piece.GetLength(1); j++)
            {
                if (piece[i, j] == 1)
                {
                    if (i > width)
                        width = i;
                    if (j > height)
                        height = j;
                }
            }
        }

        rect.Width = width;
        rect.Height = height;

        return rect;
    }


    public void Draw(SpriteBatch sb)
    {
        
        for (int y = 0; y < Board.GetLength(1); y++)
        {
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                //draw current piece                

                if (Board[x, y] == 1)
                {
                    sb.Draw(tileTexture, new Rectangle(new Point((x * 32) + xOffset, y * 32), new Point(32, 32)), Color.White);
                }
            }
        }

        var currentPiece = Pieces[CurrentPieceType][CurrentPieceRotation];
        var currentPieceColor = GetBlockColor();


            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                for (int x = 0; x < currentPiece.GetLength(0); x++)
                {
                    if (currentPiece[x, y] == 1)
                    {
                        sb.Draw(tileTexture, new Rectangle(new Point(((x + CurrentPieceX) * 32) + xOffset, (y + CurrentPieceY) * 32), new Point(32, 32)), currentPieceColor);
                    }
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