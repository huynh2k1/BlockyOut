using LevelEditor;
using UnityEngine;

public class BlockEditor : MonoBehaviour
{
    [Header("Data")]
    public BlockWrapper data;

    [Header("References")]
    [SerializeField] Cell _cellPrefab;

    [SerializeField] BlockEditor _blockPrefab;
    [SerializeField] ShapeData _shapeData;

    Cell[,] cells;
    Vector2 center;

    Vector3 _prevMousePos;
    Vector3 _curMousePos;
    Vector3 _initPos;
    Vector3 _initScale;

    Vector2Int previousDragPoint;
    Vector2Int currentDragPoint;

    private void OnMouseDown()
    {
        _initPos = transform.position;
        BlockEditor block = Instantiate(_blockPrefab, transform.parent);
        block.Initialize(_shapeData);
        block.transform.localPosition = transform.localPosition;
        this.name = "Current";

        transform.SetParent(null);
        transform.localScale = Vector3.one;

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 quan trọng

        _prevMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _prevMousePos.y = 0;

        transform.position = _initPos;
        transform.localScale = Vector3.one;

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);
        previousDragPoint = currentDragPoint;
    }

    private void OnMouseDrag()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 thêm dòng này khi drag

        _curMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _curMousePos.y = 0;

        if (_curMousePos != _prevMousePos)
        {
            _prevMousePos = _curMousePos;

            transform.position = _curMousePos;

            currentDragPoint = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z) - center + LevelEditorCtrl.I.board.OffSet/* - center + LevelEditorCtrl.I.board.OffSet*/);
            LevelEditorCtrl.I.board.Hover(currentDragPoint, _shapeData);
            if (currentDragPoint != previousDragPoint)
            {
                previousDragPoint = currentDragPoint;
            }
        }
    }

    private void OnMouseUp()
    {
        Destroy(gameObject);
    }

    public void Initialize(ShapeData shapeData)
    {
        _shapeData = shapeData;

        cells = new Cell[shapeData.rows, shapeData.columns];
        data.Rows = shapeData.rows;
        data.Cols = shapeData.columns;

        center = new Vector2(data.Cols * 0.5f, data.Rows * 0.5f);
        for (var r = 0; r < shapeData.rows; ++r)
        {
            for (var c = 0; c < shapeData.columns; ++c)
            {
                int rowIndex = shapeData.rows - 1 - r;
                bool cellValue = shapeData.board[rowIndex].column[c];
                if (cellValue)
                {
                    cells[r, c] = Instantiate(_cellPrefab, transform);
                    cells[r, c].transform.localPosition = new Vector3(c - (data.Cols - 1) / 2f, 0, r - (data.Rows - 1) / 2f);
                }
            }
        }
    }

    void Hide()
    {
        foreach (var cell in cells)
        {
            cell.Hide();
        }
    }
}
