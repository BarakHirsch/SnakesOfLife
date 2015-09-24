using System;
using System.Linq;
using Logic.Extensions;

namespace Logic.Models
{
    public class GrassBoard
    {
        public GrassCell[][] GrassCells { get; private set; }

        public int RowLength { get; private set; }
        public int ColumnLength { get; private set; }

        public Params Params { get; private set; }

        public GrassBoard(Params currParams, int rowLength, int columnLength)
        {
            Params = currParams;
            RowLength = rowLength;
            ColumnLength = columnLength;

            GrassCells = new GrassCell[RowLength][];

            for (int i = 0; i < RowLength; i++)
            {
                GrassCells[i] = new GrassCell[ColumnLength];

                for (int j = 0; j < ColumnLength; j++)
                {
                    GrassCells[i][j] = new GrassCell(Params, i, j);
                }
            }
        }

        public void UpdateGrass()
        {
            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < ColumnLength; j++)
                {
                    var grassCell = GrassCells[i][j];

                    grassCell.UpdateGrowth(CountAlive(GrassCells, i, j));
                }
            }
        }

        private int CountAlive(GrassCell[][] board, int rowIndex, int columnIndex)
        {
            int result = 0;
            GrassCell[] row;

            if (rowIndex < board.Length - 1)
            {
                row = board[rowIndex + 1];

                if (columnIndex < row.Length - 1)
                {
                    if (row[columnIndex + 1].IsAlive)
                    {
                        result++;
                    }
                }

                if (columnIndex > 0)
                {
                    if (row[columnIndex - 1].IsAlive)
                    {
                        result++;
                    }
                }

                if (row[columnIndex].IsAlive)
                {
                    result++;
                }
            }

            if (rowIndex > 0)
            {
                row = board[rowIndex - 1];

                if (columnIndex < row.Length - 1)
                {
                    if (row[columnIndex + 1].IsAlive)
                    {
                        result++;
                    }
                }

                if (columnIndex > 0)
                {
                    if (row[columnIndex - 1].IsAlive)
                    {
                        result++;
                    }
                }

                if (row[columnIndex].IsAlive)
                {
                    result++;
                }
            }

            row = board[rowIndex];

            if (columnIndex < row.Length - 1)
            {
                if (board[rowIndex][columnIndex + 1].IsAlive)
                {
                    result++;
                }
            }

            if (columnIndex > 0)
            {
                if (board[rowIndex][columnIndex - 1].IsAlive)
                {
                    result++;
                }
            }

            return result;
        }

        public bool CellEntered(int row, int column)
        {
            return GrassCells[row][column].EnteredBySnake();
        }

        public GrassCell[] GetOptionalCells(GrassCell headLocation)
        {
            var allCells = GrassCells.GetDirations(headLocation.RowIndex, headLocation.ColumnIndex).ToArray();

            var aliveCells = allCells.Where(x => x.IsAlive).ToArray();

            return aliveCells.Any() ? aliveCells : allCells;
        }
    }
}