using System;
using System.Collections.Generic;

namespace SnakesOfLife.Models
{
    public class Snake
    {
        public int GrassCellsEaten { get; set; }
        public int TurnsHasNotEaten { get; set; }

        public Queue<GrassCell> Locations { get; private set; }

        public GrassCell HeadLocation
        {
            get { return Locations.Peek(); }
        }

        public bool ShouldSplit
        {
            get { return Locations.Count == ParametersContainer.Current.SnakeLengthForSplit; }
        }

        public bool IsStarving
        {
            get { return TurnsHasNotEaten > 0 && Locations.Count == ParametersContainer.Current.SnakeLengthToStay; }
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

            if (TurnsHasNotEaten == ParametersContainer.Current.SnakeTurnToDie)
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
            Locations.Enqueue(cell);

            CheckGrassEaten(cell.EnteredBySnake());

            if (GrassCellsEaten != ParametersContainer.Current.SnakeCellsForGrow)
            {
                Locations.Dequeue();
            }
            else
            {
                GrassCellsEaten = 0;
            }
        }
    }
}