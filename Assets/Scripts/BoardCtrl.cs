using UnityEngine;
using System.Collections.Generic;
using GameConfig;
using static ShapeData;
using UnityEngine.UIElements;

public class BoardCtrl : MonoBehaviour
{
    public const float CellSpace = 0f;

    [Header("References")]
    [SerializeField] Transform _gridParents;
    [SerializeField] private CellBoard cellPrefab;
    [SerializeField] private BlocksVisualSO dataBlocks;
    [SerializeField] private Block blockPrefab;

    private CellBoard[,] cells;
    private readonly List<CellBoard> listCell = new List<CellBoard>();
    private readonly List<Block> listBlock = new List<Block>();

    private int Rows;
    private int Columns;
    private Vector2 Offset => new Vector2((Columns - 1) / 2f, (Rows - 1) / 2f);

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
        float offsetX = col - Offset.x;
        float offsetZ = row - Offset.y;
        return new Vector3(offsetX, -1, offsetZ);
    }
    #endregion

}
