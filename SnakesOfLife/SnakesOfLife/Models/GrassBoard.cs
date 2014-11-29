using System.Linq;
using SnakesOfLife.Extensions;

namespace SnakesOfLife.Models
{
    public class GrassBoard
    {
        public GrassBoard(int rowLength, int columnLength)
        {
            RowLength = rowLength;
            ColumnLength = columnLength;

            GrassCells = new GrassCell[RowLength][];

            for (int i = 0; i < RowLength; i++)
            {
                GrassCells[i] = new GrassCell[ColumnLength];

                for (int j = 0; j < ColumnLength; j++)
                {
                    GrassCells[i][j] = new GrassCell
                    {
                        IsAlive = true,
                        NeededAliveNeighborsTurnsToGrow = ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow
                    };
                }
            }
        }

        public GrassCell[][] GrassCells { get; private set; }

        public int RowLength { get; private set; }
        public int ColumnLength { get; private set; }

        public void UpdateGrass()
        {
            var predicateResult = GrassCells.GetPredicateResult(x => x.IsAlive);

            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < ColumnLength; j++)
                {
                    var grassCell = GrassCells[i][j];

                    grassCell.NeededAliveNeighborsTurnsToGrow -= predicateResult.GetNeighbors(i, j).Count(x => x);

                    if (grassCell.NeededAliveNeighborsTurnsToGrow <= 0)
                    {
                        grassCell.IsAlive = true;
                        grassCell.NeededAliveNeighborsTurnsToGrow = ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow;
                    }
                }
            }
        }
    }

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