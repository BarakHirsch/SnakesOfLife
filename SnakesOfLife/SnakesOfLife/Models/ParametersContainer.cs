namespace SnakesOfLife.Models
{
    public class Params
    {
        public static Params Current { get; set; }

        public int NeededAliveNeighborsTurnsToGrow { get; set; }

        public int SnakeCellsForGrow { get; set; }

        public int SnakeLengthForSplit { get; set; }

        public int SnakeLengthToStop { get; set; }

        public int SnakeTurnToDie { get; set; }

        public int SnakeTurnsToShrink { get; set; }
    }
}