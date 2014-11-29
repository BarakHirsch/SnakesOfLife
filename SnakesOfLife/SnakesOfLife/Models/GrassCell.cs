using System.Windows;

namespace SnakesOfLife.Models
{
    public class GrassCell
    {
        public int NeededAliveNeighborsTurnsToGrow { get; set; }

        public bool IsAlive { get; set; }
    }
}
