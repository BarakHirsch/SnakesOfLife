namespace SnakesOfLife.Models
{
    public class ParametersContainer
    {
        public static ParametersContainer Current { get; set; }

        public int NeededAliveNeighborsTurnsToGrow { get; set; }

        public int SnakeCellsForGrow { get; set; }

        public int SnakeLengthForSplit { get; set; }

        public int SnakeLengthToStay { get; set; }

        public int SnakeTurnToDie { get; set; }
    }
}