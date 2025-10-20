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
    public List<Vector2Int> hoverPoints = new List<Vector2Int>();

    int Rows;
    int Columns;
    public Vector2 OffSet => new Vector2(Rows/2f, Columns/2f);

    [SerializeField] ShapeData _shapeData;
    [SerializeField] List<BlockEditor> listBlock = new List<BlockEditor>();

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

        return true;
    }

    private void Hover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 1;
            grid[hoverPoint.y, hoverPoint.x].Hover();
        }
    }

    public void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;
            grid[hoverPoint.y, hoverPoint.x].Normal();
            //grid[hoverPoint.y, hoverPoint.x].();
        }
        hoverPoints.Clear();
    }

    public void PlaceCell(BlockEditor block)
    {
        foreach (var p in hoverPoints)
        {
            // đặt cell vào board
            data[p.y, p.x] = 2; // 2 = đã đặt block
        }
        listBlock.Add(block);   
        // Đánh dấu vào data
        // Update UI cell
        //grid[point.y, point.x].Place(); // 👉 bạn có thể làm 1 hàm đổi màu / thay sprite
    }
    //Tính toán vị trí để đặt block lên board
    public Vector3 GetSnapPosition(Vector2Int basePoint, ShapeData shapeData)
    {
        float offsetX = (shapeData.columns) / 2f;
        float offsetZ = (shapeData.rows) / 2f;

        //float x = basePoint.x - Columns / 2f + offsetX;
        //float z = basePoint.y - Rows / 2f + offsetZ;
        float x = basePoint.x + offsetX - OffSet.y;
        float z = basePoint.y + offsetZ - OffSet.x;

        return new Vector3(x, 0, z);
    }
}
