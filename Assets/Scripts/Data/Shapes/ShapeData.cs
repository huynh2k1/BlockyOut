using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;
        private int _size = 0;

        public Row() { }

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            column = new bool[_size];
            ClearRow();


        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                column[i] = false;
            }
        }
    }

    public Shape shapePrefab;

    [Header("Grid Setting")]
    public int columns = 0;
    public int rows = 0;
    public Row[] board;

    [Header("Mesh Settings")]
    public Mesh meshData;
    public Vector3 rotation;

    public void Clear()
    {
        for(var i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];
        for(var i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }
}
