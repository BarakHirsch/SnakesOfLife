using System;
using System.Collections.Generic;

namespace SnakesOfLife.Models
{
    public class Snake
    {
        public int GrassCellsEaten { get; set; }
        public int TurnsHasNotEaten { get; set; }

        public Queue<GrassCell> Locations { get; private set; }

        public GrassCell HeadLocation { get; set; }

        public bool ShouldSplit
        {
            get { return Locations.Count == Params.Current.SnakeLengthForSplit; }
        }

        public bool IsStarving
        {
            get { return TurnsHasNotEaten > 0 && Locations.Count == Params.Current.SnakeLengthToStop; }
        }

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

        public bool StarvingSnakeTurn()
        {
            CheckGrassEaten(HeadLocation.EnteredBySnake());

            if (TurnsHasNotEaten == Params.Current.SnakeTurnToDie)
            {
                return true;
            }

            return false;
        }

        private void CheckGrassEaten(bool wasCellAlive)
        {
            if (!wasCellAlive)
            {
                TurnsHasNotEaten++;
            }
            else
            {
                GrassCellsEaten++;
                TurnsHasNotEaten = 0;
            }
        }

        public void MoveSnake(GrassCell cell)
        {
            HeadLocation = cell;

            Locations.Enqueue(cell);

            CheckGrassEaten(cell.EnteredBySnake());

            if (GrassCellsEaten != Params.Current.SnakeCellsForGrow)
            {
                Locations.Dequeue();
            }
            else
            {
                GrassCellsEaten = 0;
            }

            if (TurnsHasNotEaten == Params.Current.SnakeTurnsToShrink)
            {
                Locations.Dequeue();
                TurnsHasNotEaten = 0;
            }
        }
    }
}