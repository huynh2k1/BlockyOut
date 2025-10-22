using UnityEngine;
using System.Collections.Generic;

public class BoardCtrl : MonoBehaviour
{
    int Rows;
    int Columns;
    public const float CellSpace = 0f;
    [SerializeField] Cell cellPrefab;
    [SerializeField] Cell[,] cells;
    

    public void Initialize(LevelData levelData)
    {
        Rows = levelData.Rows;
        Columns = levelData.Cols;
        InitGrid(Rows, Columns);
    }

    public void InitGrid(int rows, int cols)
    {
        Rows = rows;
        Columns = cols; 
        cells = new Cell[Rows, Columns];

        for (var r = 0; r < Rows; r++)
        {
            for(var c = 0; c < Columns; c++)
            {
                var pos = GetCellPosition(r, c);
                cells[r, c] = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cells[r, c].name = "( " + r + "_" + c + " )";
                cells[r, c].Row = r;
                cells[r, c].Col = c;
            }
        }
    }

    public void LoadCellType()
    {

    }

    private Vector3 GetCellPosition(int row, int col)
    {
        float offsetX = col - (Columns - 1) / 2f;
        float offsetZ = row - (Rows - 1) / 2f;
        return new Vector3(offsetX, 0, offsetZ);
    }

    void ClearOldGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
