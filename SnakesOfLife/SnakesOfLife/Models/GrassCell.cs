namespace SnakesOfLife.Models
{
    public class GrassCell
    {
        public int NeededAliveNeighborsTurnsToGrow { get; private set; }

        public bool IsAlive
        {
            get { return NeededAliveNeighborsTurnsToGrow == 0; }
        }

        public bool EnteredBySnake()
        {
            if (!IsAlive)
            {
                return false;
            }

            NeededAliveNeighborsTurnsToGrow = ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow;

            return true;
        }

        public void UpdateGrowth(int aliveNeighborsAtTurn)
        {
            if (IsAlive)
            {
                return;
            }

            NeededAliveNeighborsTurnsToGrow -= aliveNeighborsAtTurn;

            if (NeededAliveNeighborsTurnsToGrow <= 0)
            {
                NeededAliveNeighborsTurnsToGrow = 0;
            }
        }
    }
}
