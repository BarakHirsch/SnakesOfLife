using System.Collections.Generic;

namespace Logic.Extensions
{
    public static class MatrixEx
    {
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
    }
}