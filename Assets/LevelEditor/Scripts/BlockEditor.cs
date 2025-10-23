using GameConfig;
using LevelEditor;
using UnityEngine;

public class BlockEditor : MonoBehaviour
{
    [Header("Data")]
    public BlockWrapper data;

    [Header("References")]
    [SerializeField] BlockModel blockModel;
    [SerializeField] CellBlockEditor _cellPrefab;
    [SerializeField] BlockEditor _blockPrefab;
    [SerializeField] ShapeData _shapeData;

    public Vector3 Position => transform.position;

    CellBlockEditor[,] cells;
    Vector2 center;

    Vector3 _prevMousePos;
    Vector3 _curMousePos;

    Vector2Int previousDragPoint;
    Vector2Int currentDragPoint;

    bool IsInBoard = false;

    private void OnMouseDown()
    {
        if (IsInBoard)
        {
            CellSelectedEditor data = CellSelectedEditor.CurCellSelected;
            UpdateVisual(data.colorType);
            return;
        }

        BlockEditor block = Instantiate(_blockPrefab, transform.parent);
        block.Initialize(_shapeData);
        block.transform.localPosition = transform.localPosition;

        transform.SetParent(null);
        transform.localScale = Vector3.one;

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 quan trọng

        _prevMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _prevMousePos.y = 0;

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);
        previousDragPoint = currentDragPoint;
    }

    private void OnMouseDrag()
    {
        if (IsInBoard) return;
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 thêm dòng này khi drag

        _curMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _curMousePos.y = 0;

        if (_curMousePos != _prevMousePos)
        {
            _prevMousePos = _curMousePos;

            transform.position = _curMousePos;

            currentDragPoint = Vector2Int.RoundToInt(
                new Vector2(transform.position.x, transform.position.z)
                - center
                + LevelEditorCtrl.I.board.OffSet
            );

            LevelEditorCtrl.I.board.Hover(currentDragPoint, _shapeData);
            if (currentDragPoint != previousDragPoint)
            {
                previousDragPoint = currentDragPoint;
            }
        }
    }

    private void OnMouseUp()
    {
        if (IsInBoard)
            return;
        var board = LevelEditorCtrl.I.board;

        if (board.hoverPoints.Count == 0)
        {
            // ❌ Không có chỗ đặt hợp lệ
            Debug.Log("Không có vị trí hợp lệ, huỷ block");
            board.RemoveBlock(this);
            Destroy();
            return;
        }

        board.PlaceBlock(this);

        Vector3 snapPos = board.GetSnapPosition(currentDragPoint, _shapeData);
        transform.position = snapPos;
        IsInBoard = true;
        // Dọn hover
        board.UnHover();
    }

    public void Initialize(ShapeData shapeData)
    {
        _shapeData = shapeData;

        cells = new CellBlockEditor[_shapeData.rows, _shapeData.columns];
        data.Rows = _shapeData.rows;
        data.Cols = _shapeData.columns;

        center = new Vector2(data.Cols / 2f, data.Rows / 2f);
        if (blockModel.IsMeshValid(_shapeData))
        {
            blockModel.ChangeMesh(_shapeData);
            return;
        }
        for (var r = 0; r < _shapeData.rows; ++r)
        {
            for (var c = 0; c < _shapeData.columns; ++c)
            {
                int rowIndex = _shapeData.rows - 1 - r;
                bool cellValue = _shapeData.board[rowIndex].column[c];
                if (cellValue)
                {
                    cells[r, c] = Instantiate(_cellPrefab, transform);
                    cells[r, c].transform.localPosition = new Vector3(c - (data.Cols - 1) / 2f, 0, r - (data.Rows - 1) / 2f);
                }
            }
        }
    }

    public void UpdateVisual(ColorType colorType)
    {
        if(colorType == ColorType.None)
        {
            var board = LevelEditorCtrl.I.board;
            board.RemoveBlock(this);
            Destroy();
            return;
        }
        blockModel.ChangeColorByType(colorType);
        data.colorBlock = colorType;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
