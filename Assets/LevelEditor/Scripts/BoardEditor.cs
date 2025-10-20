using GameConfig;
using LevelEditor;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardEditor : MonoBehaviour
{
    [SerializeField] Transform _gridTransform;
    [SerializeField] List<CellEditor> listCell = new List<CellEditor>();
    [SerializeField] CellEditor _cellPrefab;

    private CellEditor[,] grid;
    private int[,] data; //0 Empty, 1 Hover, 2 Normal
    private readonly List<Vector2Int> hoverPoints = new();

    int Rows;
    int Columns;
    public Vector2 OffSet => new Vector2(Rows/2f, Columns/2f);

    [SerializeField] ShapeData _shapeData;

    public void InitGrid(int _rows, int _columns)
    {
        CleadBoard();

        Rows = _rows;
        Columns = _columns;

        grid = new CellEditor[Rows, Columns];
        data = new int[Rows, Columns];

        for (var r = 0; r < Rows; ++r)
        {
            for (var c = 0; c < Columns; ++c)
            {
                var pos = GetCellPosition(r, c, Rows, Columns);
                grid[r, c] = Instantiate(_cellPrefab, pos, Quaternion.identity, _gridTransform);
                grid[r, c].row = r;
                grid[r, c].col = r;
                grid[r, c].name = $"( {r}, {c} )";
                listCell.Add(grid[r, c]);

                if (r == 0 || c == 0 || r == Rows - 1 || c == Columns - 1)
                {
                    grid[r, c].UpdateUI(CellBoardType.Border);
                }
                else
                {
                    grid[r, c].UpdateUI(CellBoardType.Field);
                }
            }
        }
    }

    private Vector3 GetCellPosition(int row, int col, int Rows, int Cols)
    {
        float offsetX = col - (Cols - 1) / 2f;
        float offsetZ = row - (Rows - 1) / 2f;
        return new Vector3(offsetX, -1, offsetZ);
    }


    public void CleadBoard()
    {
        foreach (var cell in listCell)
        {
            cell.Destroy();
        }
    }

    public void Hover(Vector2Int point, ShapeData shapeData)
    {
        _shapeData = shapeData;
        UnHover();
        HoverPoints(point);
        if (hoverPoints.Count > 0)
        {
            Hover();
        }
    }

    private void HoverPoints(Vector2Int point)
    {
        //Duyệt mảng 2 chiều ô nào có giá trị (true) thì hover

        for(var r = 0; r < _shapeData.rows; r++)
        {
            for(var c = 0; c < _shapeData.columns; c++)
            {
                int rowIndex = _shapeData.rows - 1 - r;
                bool cellValue = _shapeData.board[rowIndex].column[c];
                if (cellValue == true)
                {
                    var hoverPoint = point + new Vector2Int(c, r);

                    if (IsValidPoint(hoverPoint) == false)
                    {
                        hoverPoints.Clear();
                        return;
                    }

                    hoverPoints.Add(hoverPoint);
                }
                else
                {
                    Debug.Log("False");
                }
            }
        }
    }

    bool IsValidPoint(Vector2Int point)
    {
        if (point.x < 1 || Rows - 1 <= point.x) return false;

        if (point.y < 1 || Columns - 1 <= point.y) return false;

        if (data[point.y, point.x] > 0) return false;

        return false;
    }

    private void Hover()
    {
        Debug.Log("HOVER");
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 1;
            grid[hoverPoint.y, hoverPoint.x].Hover();
        }
    }

    private void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;
            //grid[hoverPoint.y, hoverPoint.x].();
        }
        hoverPoints.Clear();
    }
}
