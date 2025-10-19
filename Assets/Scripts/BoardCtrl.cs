using UnityEngine;
using System.Collections.Generic;

public class BoardCtrl : MonoBehaviour
{
    public int Rows;
    public int Columns;
    public const float CellSpace = 0f;
    [SerializeField] Cell cellPrefab;
    [SerializeField] Cell[,] cells;


    private void Start()
    {
        
    }

    public void InitGrid()
    {
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
