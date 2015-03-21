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

        public Params Params { get; private set; }

        public bool ShouldSplit
        {
            get { return Locations.Count == Params.SnakeLengthForSplit; }
        }

        public bool IsStarving
        {
            get { return TurnsHasNotEaten > 0 && Locations.Count == Params.SnakeLengthToStop; }
        }

        public Snake(Params currParams)
        {
            Params = currParams;
            Locations = new Queue<GrassCell>();
        }
        
        public Snake SplitSnake()
        {
            var originalLength = Locations.Count;

            var partsToMove = (int) Math.Ceiling(originalLength/2.0);

            var splitSnake = new Snake(Params);

            for (var i = 0; i < partsToMove; i++)
            {
                splitSnake.AddNewCell(Locations.Dequeue());
            }

            return splitSnake;
        }

        public bool StarvingSnakeTurn()
        {
            CheckGrassEaten(HeadLocation.EnteredBySnake());

            if (TurnsHasNotEaten == Params.SnakeTurnToDie)
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
            AddNewCell(cell);

            CheckGrassEaten(cell.EnteredBySnake());

            if (GrassCellsEaten <= Params.SnakeCellsForGrow)
            {
                Locations.Dequeue();
            }
            else
            {
                GrassCellsEaten = 0;
            }

            if (Locations.Count > Params.SnakeLengthToStop && TurnsHasNotEaten == Params.SnakeTurnsToShrink)
            {
                Locations.Dequeue();
                TurnsHasNotEaten = 0;
            }
        }

        public void AddNewCell(GrassCell cell)
        {
            HeadLocation = cell;
            Locations.Enqueue(cell);
        }
    }
}