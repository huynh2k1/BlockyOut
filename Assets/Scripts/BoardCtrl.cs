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
    private List<Block> listBlock = new List<Block>();

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

            b.Initialize(data[i].position.ToVector3(), data[i].id, data[i].colorType, data[i].blockDir, dataBlocks.GetShapeDataByID(data[i].id));

            listBlock.Add(b);
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

    private void PlaceBlock(List<Vector2Int> hoverPoints)
    {
        foreach (var p in hoverPoints)
        {
            data[p.y, p.x] = 2;
        }
    }

    public List<Vector2Int> GetOccupiedPoints(Vector2Int point, ShapeData shapeData)
    {
        var points = new List<Vector2Int>();

        //Duyệt mảng 2 chiều ô nào có giá trị (true) thì hover
        for (var r = 0; r < shapeData.rows; r++)
        {
            for (var c = 0; c < shapeData.columns; c++)
            {
                int rowIndex = shapeData.rows - 1 - r; //đảo ngược hàng để hiển thị đúng
                bool cellValue = shapeData.board[rowIndex].column[c];
                if (!cellValue) continue;

                // không cộng offset nữa, chỉ cộng tương đối
                Vector2Int hoverPoint = new Vector2Int(point.x + c, point.y + r);
                if (!IsValidPoint(hoverPoint))
                {

                    points.Clear();
                    return points;
                }

                points.Add(hoverPoint);
            }
        }
        return points;
    }

    public void PlaceBlock(Vector2Int point, ShapeData shapeData)
    {
        UnHover();
        var points = GetOccupiedPoints(point, shapeData);
        if (points.Count == 0)
            return;
        PlaceBlock(points);
    }
    public void Hover(Vector2Int point, ShapeData shapeData)
    {
        UnHover();
        HoverPoints(point, shapeData);
        if (hoverPoints.Count > 0)
        {
            Hover();
        }
    }

    public void ClearDataPoint(List<Vector2Int> points)
    {
        if (points.Count == 0)
            return;
        foreach (var p in points)
        {
            data[p.y, p.x] = 0;
        }
    }

    private void HoverPoints(Vector2Int point, ShapeData shapeData)
    {
        //Duyệt mảng 2 chiều ô nào có giá trị (true) thì hover
        for (var r = 0; r < shapeData.rows; r++)
        {
            for (var c = 0; c < shapeData.columns; c++)
            {
                int rowIndex = shapeData.rows - 1 - r; //đảo ngược hàng để hiển thị đúng
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

        if (data[point.y, point.x] == 2) return false;

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
        if (hoverPoints.Count == 0)
            return;
        foreach (var p in hoverPoints)
        {
            data[p.y, p.x] = 0;
            cells[p.y, p.x].Hover(false);
            //grid[hoverPoint.y, hoverPoint.x].();
        }
        hoverPoints.Clear();
    }

    //Tính toán vị trí để đặt block lên board
    public Vector3 GetSnapPosition(Vector2Int basePoint, ShapeData shapeData)
    {
        float offsetX = (shapeData.columns) / 2f;
        float offsetZ = (shapeData.rows) / 2f;

        float x = basePoint.x + offsetX - Offset.x;
        float z = basePoint.y + offsetZ - Offset.y;

        return new Vector3(x, 0, z);
    }


    public void CanExit(Block block)
    {
        if (hoverPoints == null || hoverPoints.Count == 0)
            return;

        foreach (var p in hoverPoints.ToArray())
        {
            //Rìa trên 
            if (p.y == Rows - 2)
            {
                CheckEdge(block, Rows - 1, p.x, block.data.Cols, 1, 0, true);
            }

            //Rìa dưới
            if (p.y == 1)
            {
                CheckEdge(block, 0, p.x, block.data.Cols, -1, 0, true);
            }

            //Rìa trái
            if (p.x == 1)
            {
                CheckEdge(block, p.y, 0, block.data.Rows, 0, -1, false);
            }

            //Rìa phải
            if (p.x == Columns - 2)
            {
                CheckEdge(block, p.y, Columns - 1, block.data.Rows, 0, 1, false);
            }
        }
    }

    void CheckEdge(Block block, int cellBorderY, int cellBorderX, int blockSize, int dR, int dC, bool isHorizontal)
    {
        //Tìm ô rìa có type là border -> cửa ra
        CellBoard cell = cells[cellBorderY, cellBorderX];

        //Nếu như block không cùng màu với cửa => không thể ra  
        if (!CheckHaveDoor(cell, block.data.colorBlock))
            return;

        //Danh sách các ô type = border
        List<CellBoard> doorEdges = GetDoorSize(cell, isHorizontal);

        //Nếu kích thước block > kích thước cửa => không thể ra
        if (blockSize > doorEdges.Count)
        {
            Debug.Log($"Block size {blockSize} bigger than door {doorEdges.Count}");
            return;
        }

        //Kiểm tra block có bị chặn bởi block khác hay không
        if (IsBlockCanExit(block, dR, dC))
        {
            Debug.Log("Can Exit");
            foreach (CellBoard c in doorEdges)
            {
                c.PlayCrushingEffect();
            }
            BlockOut(block, cell, dR, dC);
        }
        else
            Debug.Log("Bị chặn");
    }

    void BlockOut(Block block, CellBoard cellBorder, int dR, int dC)
    {
        block.StartOut();
        Vector3 targetPos = Vector3.zero;
        if (dR != 0)
        {
            float distance = Mathf.Abs(cellBorder.GetPosition.z - block.GetPosition.z);
            targetPos = new Vector3(block.GetPosition.x, block.GetPosition.y, distance * 3f * dR);
        }
        else if (dC != 0)
        {
            float distance = Mathf.Abs(cellBorder.GetPosition.x - block.GetPosition.x);
            targetPos = new Vector3(distance * 3f * dC, block.GetPosition.y, block.GetPosition.z);
        }

        Debug.Log($"Distance: {targetPos}");
        block.BlockOut(targetPos, () =>
        {
            CountRemainingBlocks(block);
        });
    }

    void CountRemainingBlocks(Block block)
    {
        if (listBlock.Contains(block))
        {
            listBlock.Remove(block);
            if(listBlock.Count == 0)
            {
                GameCtrl.I.GameWin();
            }
        }
    }

    private bool IsBlockCanExit(Block block, int dR, int dC)
    {
        foreach(var pos in hoverPoints)
        {
            if(IsPointBlocked(block, pos, dR, dC) == false)
            {
                return false;
            }
        }
        return true;
    }

    //Kiểm tra 1 ô dữ liệu trong grid có bị chặn hay không có bị chặn hay không
    //0 : không bị chặn, 2 : bị chặn
    //isHorizon : true -> kiểm tra chiều ngang có bị chặn hay không
    //isHorizon : false -> kiểm tra chiều dọc có bị chặn hay không
    //step : 1 - duyệt tăng dần, -1 - duyệt giảm dần
    private bool IsPointBlocked(Block block, Vector2Int point, int dR, int dC)
    {
        int r = point.y + dR;
        int c = point.x + dC;

        while (true)
        {
            // Nếu ra khỏi biên → không bị chặn
            if (r < 0 || r >= Rows || c < 0 || c >= Columns)
                return true;

            Vector2Int pos = new Vector2Int(c, r);
            CellBoard cell = cells[r, c];

            // Nếu gặp border khác màu → chặn ngay
            if (cell.cellType == CellBoardType.Border && cell.colorType != block.data.colorBlock)
            {
                Debug.Log($"Cell: Row {r}, Col {c} CHẶN (biên khác màu)");
                return false;
            }

            // Nếu gặp block cố định khác block đang hover → chặn
            if (data[r, c] == 2 && !hoverPoints.Contains(pos))
            {
                Debug.Log($"Cell: Row {r}, Col {c} CHẶN (block khác)");
                return false;
            }

            // Tiếp tục đi tới ô kế tiếp theo hướng
            r += dR;
            c += dC;
        }
    }

    //kiểm tra có cửa và cửa có cùng màu hay không
    //nếu không có cửa hoặc cửa khác màu => false
    //nếu có => true
    bool CheckHaveDoor(CellBoard cellEdge, ColorType colorBlock)
    {
        if (cellEdge.cellType != CellBoardType.Border
        || cellEdge.colorType != colorBlock)
            return false;

        return true;
    }

    //truyền vào 1 ô là door và trả về kích thước của cửa
    private List<CellBoard> GetDoorSize(CellBoard doorCellPos, bool isHorizontal)
    {
        List<CellBoard> doorPoints = new List<CellBoard>();
        doorPoints.Add(doorCellPos);

        int row = doorCellPos.Row;
        int col = doorCellPos.Col;
        ColorType colorDoor = cells[row, col].colorType;
        CellBoard cell = null;
        if (isHorizontal)
        {
            //Duyệt phải
            for (var i = col + 1; i < Columns; i++)
            {
                cell = cells[row, i];
                if (cell.cellType != CellBoardType.Border
                    || cell.colorType != colorDoor)
                {
                    break;
                }
                doorPoints.Add(cell);
            }
            //Duyệt trái 
            for (var i = col - 1; i >= 0; i--)
            {
                cell = cells[row, i];
                if (cell.cellType != CellBoardType.Border
                    || cell.colorType != colorDoor)
                {
                    break;
                }
                doorPoints.Add(cell);
            }
        }
        else
        {
            //Duyệt trên
            for (var i = row + 1; i < Rows; i++)
            {
                cell = cells[i, col];
                if (cell.cellType != CellBoardType.Border
                    || cell.colorType != colorDoor)
                {
                    break;
                }
                doorPoints.Add(cell);
            }
            //Duyệt xuống
            for (var i = row - 1; i >= 0; i--)
            {
                cell = cells[i, col];
                if (cell.cellType != CellBoardType.Border
                    || cell.colorType != colorDoor)
                {
                    break;
                }
                doorPoints.Add(cell);
            }
        }
        return doorPoints;

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
        return new Vector3(offsetX, -0.6f, offsetZ);
    }
    #endregion

}
