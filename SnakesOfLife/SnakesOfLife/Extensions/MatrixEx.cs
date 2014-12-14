using System;
using System.Collections.Generic;

namespace SnakesOfLife.Extensions
{
    public static class MatrixEx
    {
        public static bool[][] GetPredicateResult<T>(this T[][] board, Func<T, bool> predicate)
        {
            var bools = new bool[board.Length][];

            for (int i = 0; i < board.Length; i++)
            {
                var row = new bool[board[i].Length];

                for (int j = 0; j < row.Length; j++)
                {
                    row[j] = predicate(board[i][j]);
                }

                bools[i] = row;
            }

            return bools;
        }

        public static IEnumerable<T> GetDirations<T>(this T[][] board, int rowIndex, int columnIndex)
        {
            if (rowIndex < board.Length - 1)
            {
                yield return board[rowIndex + 1][columnIndex];
            }

            if (rowIndex > 0)
            {
                yield return board[rowIndex - 1][columnIndex];
            }

            if (columnIndex < board[rowIndex].Length - 1)
            {
                yield return board[rowIndex][columnIndex + 1];
            }

            if (columnIndex > 0)
            {
                yield return board[rowIndex][columnIndex - 1];
            }
        }

        public static IEnumerable<T> GetNeighbors<T>(this T[][] board, int rowIndex, int columnIndex)
        {
            T[] row;

            if (rowIndex < board.Length - 1)
            {
                row = board[rowIndex + 1];

                if (columnIndex < row.Length - 1)
                {
                    yield return row[columnIndex + 1];
                }

                if (columnIndex > 0)
                {
                    yield return row[columnIndex - 1];
                }

                yield return row[columnIndex];
            }

            if (rowIndex > 0)
            {
                row = board[rowIndex - 1];

                if (columnIndex < row.Length - 1)
                {
                    yield return row[columnIndex + 1];
                }

                if (columnIndex > 0)
                {
                    yield return row[columnIndex - 1];
                }

                yield return row[columnIndex];
            }

            row = board[rowIndex];

            if (columnIndex < row.Length - 1)
            {
                yield return board[rowIndex][columnIndex + 1];
            }

            if (columnIndex > 0)
            {
                yield return board[rowIndex][columnIndex - 1];
            }
        }
    }
}