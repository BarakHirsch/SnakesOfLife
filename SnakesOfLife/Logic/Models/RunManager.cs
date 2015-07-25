using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Logic.Models
{
    public class RunManager
    {
        private readonly Random _random;

        public List<Snake> Snakes { get; set; }
        public GrassBoard GrassBoard { get; set; }
        public int TurnsCount { get; private set; }
        public bool HasEnded { get; set; }

        public Params Params { get; private set; }

        public RunManager(Params currParams, int rowLength, int columnLength, Random random)
        {
            Params = currParams;
            _random = random;

            Snakes = new List<Snake>();
            GrassBoard = new GrassBoard(Params, rowLength, columnLength);

            var snake = new Snake(Params);

            var grassCell = GrassBoard.GrassCells[_random.Next(rowLength)][_random.Next(columnLength)];
            snake.AddNewCell(grassCell);

            for (int i = 0; i < 3; i++)
            {
                grassCell = GetRandomGrassCell(grassCell);
                snake.AddNewCell(grassCell);
            }

            Snakes.Add(snake);
        }

        public void RunToEnd(BackgroundWorker cancellationToken)
        {
            while (!HasEnded && !cancellationToken.CancellationPending)
            {
                RunTurn();
            }
        }

        public void RunTurn()
        {
            if (!Snakes.Any())
            {
                HasEnded = true;
            }
            else
            {
                TurnsCount++;

                GrassBoard.UpdateGrass();

                MoveSnakes();
            }
        }

        private void MoveSnakes()
        {
            foreach (var snake in Snakes.ToArray())
            {
                if (snake.TurnsHasNotEaten == Params.SnakeTurnToDie)
                {
                    Snakes.Remove(snake);
                }

                if (snake.IsStarving)
                {
                    var isDead = snake.StarvingSnakeTurn();

                    if (isDead)
                    {
                        Snakes.Remove(snake);
                    }
                }
                else
                {
                    var headLocation = snake.HeadLocation;

                    snake.MoveSnake(GetRandomGrassCell(headLocation));
                }

                if (snake.ShouldSplit)
                {
                    Snakes.Add(snake.SplitSnake());
                }
            }
        }

        private GrassCell GetRandomGrassCell(GrassCell headLocation)
        {
            var grassCells = GrassBoard.GetOptionalCells(headLocation).ToArray();

            return grassCells[_random.Next(grassCells.Length)];
        }

        public void AddSnake()
        {
            var snake = new Snake(Params);

            var grassCells = GrassBoard.GrassCells[0];
            snake.Locations.Enqueue(grassCells[0]);
            snake.Locations.Enqueue(grassCells[1]);
            snake.Locations.Enqueue(grassCells[2]);
            snake.HeadLocation = grassCells[2];

            Snakes.Add(snake);
        }
    }
}