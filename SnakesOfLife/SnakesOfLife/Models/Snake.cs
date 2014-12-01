using System;
using System.Collections.Generic;

namespace SnakesOfLife.Models
{
    public class Snake
    {
        public int GrassEaten { get; set; }

        public Queue<GrassCell> Locations { get; private set; }

        public Snake()
        {
            Locations = new Queue<GrassCell>();
        }

        public Snake SplitSnake()
        {
            var originalLength = Locations.Count;

            var partsToMove = (int) Math.Ceiling(originalLength/2.0);

            var splitSnake = new Snake();

            for (var i = 0; i < partsToMove; i++)
            {
                splitSnake.Locations.Enqueue(Locations.Dequeue());
            }

            return splitSnake;
        }

        public void SnakeMove(GrassCell cell)
        {
            cell.EnteredBySnake();
        }
    }
}