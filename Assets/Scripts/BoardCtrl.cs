using UnityEngine;
using System.Collections.Generic;
using GameConfig;
using static ShapeData;
using UnityEngine.UIElements;

public class BoardCtrl : MonoBehaviour
{
    public static BoardCtrl I;
    public const float CellSpace = 0f;

    [Header("References")]
    [SerializeField] Transform _gridParents;
    [SerializeField] private CellBoard cellPrefab;
    [SerializeField] private BlocksVisualSO dataBlocks;
    [SerializeField] private Block blockPrefab;

    private CellBoard[,] cells;
    private int[,] data; //0 Empty, 1 Hover, 2 Normal

    private List<Vector2Int> hoverPoints = new List<Vector2Int>();
    private readonly List<CellBoard> listCell = new List<CellBoard>();
    private readonly List<Block> listBlock = new List<Block>();

    private int Rows;
    private int Columns;
    public Vector2 Offset => new Vector2((Columns) / 2f, (Rows) / 2f);

    private void Awake()
    {
        I = this;
    }

    public void Initialize(LevelData levelData)
    {
        Rows = levelData.Rows;
        Columns = levelData.Cols;

        InitGrid(Rows, Columns);
        LoadCellType(levelData.Cells);

        InitBlocks(levelData.Blocks);
    }

    public void InitGrid(int rows, int cols)
    {
        ClearOldGrid(); 

        Rows = rows;
        Columns = cols; 

        cells = new CellBoard[Rows, Columns];
        data = new int[Rows, Columns];

        for (var r = 0; r < Rows; r++)
        {
            for(var c = 0; c < Columns; c++)
            {
                var pos = GetCellPosition(r, c);
                cells[r, c] = Instantiate(cellPrefab, pos, Quaternion.identity, _gridParents);
                cells[r, c].name = "( " + r + "_" + c + " )";
                cells[r, c].Row = r;
                cells[r, c].Col = c;
                listCell.Add(cells[r, c]);

                CellBoard cell = cells[r, c];
                SetupBorder(r, c, cell);
            }
        }
    }

    void ClearOldGrid()
    {
        if(listCell.Count <= 0)
            return;
        foreach (var c in listCell)
        {
            Destroy(c.gameObject);
        }
        listCell.Clear();
    }

    public void InitBlocks(BlockData[] data)
    {
        ClearBlocks();

        for(var i = 0; i < data.Length; i++)
        {
            var b = Instantiate(blockPrefab, transform);
            b.transform.position = data[i].position.ToVector3();
            b.Initialize(data[i].id, data[i].colorType, data[i].blockDir, dataBlocks.GetShapeDataByID(data[i].id));
        }
    }

    void ClearBlocks()
    {
        if (listBlock.Count <= 0)
            return;
        foreach(var b in listBlock)
        {
            b.Destroy();
        }
        listBlock.Clear();
    }

    #region Block Logic
    public void Hover(Vector2Int point, ShapeData shapeData)
    {
        UnHover();
        HoverPoints(point, shapeData);
        if (hoverPoints.Count > 0)
        {
            Hover();
        }
    }

    private void HoverPoints(Vector2Int point, ShapeData shapeData)
    {
        //Duyệt mảng 2 chiều ô nào có giá trị (true) thì hover
        Debug.Log($"Point: {point}");
        for (var r = 0; r < shapeData.rows; r++)
        {
            for (var c = 0; c < shapeData.columns; c++)
            {
                int rowIndex = shapeData.rows - 1 - r;
                bool cellValue = shapeData.board[rowIndex].column[c];
                if (!cellValue) continue;

                // không cộng offset nữa, chỉ cộng tương đối
                Vector2Int hoverPoint = new Vector2Int(point.x + c, point.y + r);

                if (!IsValidPoint(hoverPoint))
                {
                    hoverPoints.Clear();
                    return;
                }

                hoverPoints.Add(hoverPoint);
            }
        }
    }

    bool IsValidPoint(Vector2Int point)
    {
        // point.x = col, point.y = row
        if (point.y < 1 || point.y >= Rows - 1) return false;
        if (point.x < 1 || point.x >= Columns - 1) return false;

        if (data[point.y, point.x] > 0) return false;

        return true;
    }

    private void Hover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 1;
            cells[hoverPoint.y, hoverPoint.x].Hover(true);
        }
    }

    public void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;
            cells[hoverPoint.y, hoverPoint.x].Hover(false);
            //grid[hoverPoint.y, hoverPoint.x].();
        }
        hoverPoints.Clear();
    }

    //Tính toán vị trí để đặt block lên board
    public Vector3 GetSnapPosition(Vector2Int basePoint, ShapeData shapeData)
    {
        float offsetX = (shapeData.columns) / 2f;
        float offsetZ = (shapeData.rows) / 2f;

        //float x = basePoint.x - Columns / 2f + offsetX;
        //float z = basePoint.y - Rows / 2f + offsetZ;
        float x = basePoint.x + offsetX - Offset.x;
        float z = basePoint.y + offsetZ - Offset.y;

        return new Vector3(x, 0, z);
    }
    #endregion

    #region Cell Logic
    private void SetupBorder(int r, int c, CellBoard cell)
    {
        bool isTop = r == Rows - 1;
        bool isBottom = r == 0;
        bool isLeft = c == 0;
        bool isRight = c == Columns - 1;

        // ✅ 4 góc — không có border
        if ((isTop && isLeft) || (isTop && isRight) ||
            (isBottom && isLeft) || (isBottom && isRight))
        {
            cell.EmptyCell();
            return;
        }

        if (isLeft)
            cell.border.SetRotation(0);
        else if (isRight)
            cell.border.SetRotation(3);
        else if (isTop)
            cell.border.SetRotation(2);
        else if (isBottom)
            cell.border.SetRotation(1);
    }

    public void LoadCellType(CellData[] data)
    {
        for(int i = 0; i < listCell.Count; i++)
        {
            listCell[i].Initialize(data[i].cellType, data[i].colorType);
        }
    }

    private Vector3 GetCellPosition(int row, int col)
    {
        float offsetX = col - (Columns - 1) / 2f;
        float offsetZ = row - (Rows - 1) / 2f;
        return new Vector3(offsetX, -0.7f, offsetZ);
    }
    #endregion

}
