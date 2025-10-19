public static class ArrUtils
{
    public static void ReverseRows<T>(T[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        for (var r = 0; r < rows / 2; r++)
        {
            var bottomRow = rows - 1 - r;
            for (var c = 0; c < cols; ++c)
            {
                (arr[r, c], arr[bottomRow, c]) = (arr[bottomRow, c], arr[r, c]);
            }
        }
    }

    public static void ReverseCols<T>(T[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        for (var c = 0; c < cols / 2; ++c)
        {
            var opposite = cols - 1 - c;
            for (var r = 0; r < rows; ++r)
            {
                (arr[r, c], arr[r, opposite]) = (arr[r, opposite], arr[r, c]);
            }
        }
    }

    public static void ReverseArray<T>(T[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        for (int r = 0; r < rows / 2; r++)
        {
            for (int c = 0; c < cols / 2; ++c)
            {
                int oppositeRow = rows - 1 - c;
                int oppositeCol = cols - 1 - c;

            }
        }
    }

}