using UnityEngine;
using System.Collections.Generic;
using GameConfig;

public class BoardCtrl : MonoBehaviour
{
    int Rows;
    int Columns;
    public const float CellSpace = 0f;
    [SerializeField] CellBoard cellPrefab;
    [SerializeField] CellBoard[,] cells;
    [SerializeField] List<CellBoard> listCell = new List<CellBoard>();
    
    public void Initialize(LevelData levelData)
    {
        Rows = levelData.Rows;
        Columns = levelData.Cols;
        InitGrid(Rows, Columns);
        LoadCellType(levelData.Cells);
    }

    public void InitGrid(int rows, int cols)
    {
        Rows = rows;
        Columns = cols; 
        cells = new CellBoard[Rows, Columns];

        for (var r = 0; r < Rows; r++)
        {
            for(var c = 0; c < Columns; c++)
            {
                var pos = GetCellPosition(r, c);
                cells[r, c] = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cells[r, c].name = "( " + r + "_" + c + " )";
                cells[r, c].Row = r;
                cells[r, c].Col = c;
                listCell.Add(cells[r, c]);

                CellBoard cell = cells[r, c];

                if (r == 0 && c == 0)
                    continue;
                else if (r == Rows - 1 && c == 0)
                    continue;
                else if (r == 0 && c == Columns - 1)
                    continue;
                else if (r == Rows - 1 && c == Columns - 1)
                    continue;

                // Rìa trái
                else if (c == 0)
                    cell.border.SetRotation(0);

                // Rìa phải
                else if (c == Columns - 1)
                    cell.border.SetRotation(3);

                // Rìa trên
                else if (r == Rows - 1)
                    cell.border.SetRotation(2);

                // Rìa dưới
                else if (r == 0)
                    cell.border.SetRotation(1);
            }
        }
    }

    public void LoadCellType(CellData[] data)
    {
        //
        for(int i = 0; i < listCell.Count; i++)
        {
            listCell[i].Initialize(data[i].cellType, data[i].colorType);
        }
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
