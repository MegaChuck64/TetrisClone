using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.GameObjects;

public class Tetris
{
    public int[,] Board { get; set; } = new int[10, 20];
    public int[,] CurrentPiece { get; set; } = new int[4, 4];
    public int[,] NextPiece { get; set; } = new int[4, 4];

    public int CurrentPieceX { get; set; } = 3;
    public int CurrentPieceY { get; set; } = 0;

    public int NextPieceX { get; set; } = 3;
    public int NextPieceY { get; set; } = 0;

    //Time = (0.8-((Level-1)*0.007))(Level-1)
    public float StepSpeed { get; set; } = 1f;
    private float stepTimer = 0f;

    private Texture2D tileTexture;

    public List<int[,]> Pieces { get; set; } = new List<int[,]>()
    {
        //0
        new int[4, 4]
        {
            {0,0,0,0},
            {0,1,1,0},
            {0,1,1,0},
            {0,0,0,0}
        },

        //1
        new int[4, 4]
        {
            {0,0,0,0},
            {0,0,1,0},
            {0,1,1,1},
            {0,0,0,0}
        },

        //2
        new int[4, 4]
        {
            {0,0,0,0},
            {0,1,0,0},
            {0,1,1,1},
            {0,0,0,0}
        },

        //3
        new int[4, 4]
        {
            {0,0,0,0},
            {0,0,1,1},
            {0,1,1,0},
            {0,0,0,0}
        },

        //4
        new int[4, 4]
        {
            {0,0,0,0},
            {0,1,1,0},
            {0,0,1,1},
            {0,0,0,0}
        },

        //5
        new int[4, 4]
        {
            {0,0,0,0},
            {0,0,1,0},
            {0,1,1,0},
            {0,1,0,0}
        },

        //6
        new int[4, 4]
        {
            {0,0,0,0},
            {0,1,0,0},
            {0,1,1,0},
            {0,0,1,0}
        },
    };


    public Tetris(BaseGame game)
    {
        CurrentPiece = GetRandomPiece();
        NextPiece = GetRandomPiece();

        tileTexture = game.Content.Load<Texture2D>(@"sprites\tile");
    }

    public void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState)
    {
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
        if (keyState.IsKeyDown(Keys.Up))
        {
            //rotate
        }

        stepTimer += dt;

        if (stepTimer >= StepSpeed)
        {
            stepTimer = 0f;
            CurrentPieceY++;
        }
    }

    public void Draw(SpriteBatch sb)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                if (Board[x, y] == 1)
                {
                    sb.Draw(tileTexture, new Rectangle(new Point(x * 32, y * 32), new Point(32, 32)), Color.White);
                }
            }
        }
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (CurrentPiece[x, y] == 1)
                    sb.Draw(tileTexture, new Rectangle(new Point((x + CurrentPieceX) * 32, (y + CurrentPieceY) * 32), new Point(32, 32)), Color.White);
            }
        }
    }

    public int[,] GetRandomPiece()
    {
        var random = new Random();
        var choice = random.Next(0, 7);

        var piece = Pieces[choice];

        return piece;
    }

}