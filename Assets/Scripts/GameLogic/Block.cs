using UnityEngine;
using GameConfig;
using UnityEngine.UI;
public class Block : MonoBehaviour
{
    public BlockWrapper data;
    

    [SerializeField] Cell _cellPrefab;
    [SerializeField] Cell[,] cells;

    public ShapeData _shapeData;
    Vector2 center;


    private void Start()
    {
        Initialize(_shapeData);
    }

    public void Initialize(ShapeData shapeData)
    {
        //_initPos = transform.position;
        //_initScale = transform.localScale;
        cells = new Cell[shapeData.rows, shapeData.columns];
        var Rows = shapeData.rows;
        var Cols = shapeData.columns;   
        for (var r = 0; r < shapeData.rows; ++r)
        {
            for (var c = 0; c < shapeData.columns; ++c)
            {
                int rowIndex = shapeData.rows - 1 - r;
                //int colIndex = shapeData.columns - 1 - c;
                bool cellValue = shapeData.board[rowIndex].column[c];
                if (cellValue)
                {
                    cells[r, c] = Instantiate(_cellPrefab, transform);
                    cells[r, c].transform.localPosition = new Vector3(c - Cols / 2 + 1, 0,r - Rows / 2 + 1);
                }
            }
        }
    }

    void Hide()
    {
        foreach(var cell in cells)
        {
            cell.Hide();
        }
    }
}

public struct BlockWrapper
{
    //Block Property
    public int blockID;
    public ColorType colorBlock;
    public BlockDir blockDir;
    public int Rows { get; set; }
    public int Cols { get; set; }
}
