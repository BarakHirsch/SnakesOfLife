namespace SnakesOfLife.Models
{
    public class Params
    {
        public int NeededAliveNeighborsTurnsToGrow { get; set; }

        public int SnakeCellsForGrow { get; set; }

        public int SnakeLengthForSplit { get; set; }

        public int SnakeLengthToStop { get; set; }

        public int SnakeTurnToDie { get; set; }

        public int SnakeTurnsToShrink { get; set; }

        public Params Clone()
        {
            return (Params) MemberwiseClone();
        }
    }
}