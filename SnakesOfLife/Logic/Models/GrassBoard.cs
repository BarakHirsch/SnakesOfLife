﻿using System.Linq;
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
            var predicateResult = GrassCells.GetPredicateResult(x => x.IsAlive);

            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < ColumnLength; j++)
                {
                    var grassCell = GrassCells[i][j];

                    grassCell.UpdateGrowth(predicateResult.GetNeighbors(i, j).Count(x => x));
                }
            }
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