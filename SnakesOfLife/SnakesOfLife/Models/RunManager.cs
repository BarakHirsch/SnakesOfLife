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

        public RunManager()
        {
            _random = new Random();
        }

        public void RunTurn()
        {
            GrassBoard.UpdateGrass();

            foreach (var snake in Snakes.ToArray())
            {
                if (snake.TurnsHasNotEaten == ParametersContainer.Current.SnakeTurnToDie)
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