using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakesOfLife.Models
{
    public class RunManager
    {
        private readonly Random _random;

        public List<Snake> Snakes { get; set; }
        public GrassBoard GrassBoard { get; set; }
        public int TurnsCount { get; private set; }

        public RunManager(int rowLength, int columnLength)
        {
            _random = new Random();

            Snakes = new List<Snake>();
            GrassBoard = new GrassBoard(rowLength, columnLength);
        }

        public bool RunTurn()
        {
            if (!Snakes.Any())
            {
                return false;
            }

            TurnsCount++;

            GrassBoard.UpdateGrass();

            MoveSnakes();

            return true;
        }

        private void MoveSnakes()
        {
            foreach (var snake in Snakes.ToArray())
            {
                if (snake.TurnsHasNotEaten == Params.Current.SnakeTurnToDie)
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
                    var grassCells = GrassBoard.GetOptionalCells(snake.HeadLocation).ToArray();

                    snake.MoveSnake(grassCells[_random.Next(grassCells.Length)]);
                }

                if (snake.ShouldSplit)
                {
                    Snakes.Add(snake.SplitSnake());
                }
            }
        }
    }
}