using UnityEngine;
using GameConfig;
using UnityEngine.UI;
using System;
public class Block : MonoBehaviour
{
    Rigidbody rb;
    public BlockWrapper data;
    private Shape shape;
    ShapeData _shapeData;

    Vector2 center;
    Vector3 _prevMousePos;
    Vector3 _curMousePos;
    Vector3 _targetPos;
    bool _isDragging = false;

    Vector2Int prevGridPos;
    Vector2Int curGridPos;

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;               // Bình thường không chịu vật lý
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Initialize(int id, ColorType colorType, BlockDir blockDir, ShapeData shapeData)
    {
        _shapeData = shapeData; 
        shape = Instantiate(shapeData.shapePrefab, transform);
        center = new Vector2(shapeData.columns / 2f, shapeData.rows / 2f);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, shapeData.rotation.z, transform.rotation.z));

        shape.HandleOnMouseDown(HandleOnMouseDown);
        shape.HandleOnMouseDrag(HandleOnMouseDrag);
        shape.HandleOnMouseUp(HandleOnMouseUp);

        data.blockID = id;
        data.colorBlock = colorType;    
        data.blockDir = blockDir;

        ChangeColorByType();
    }

    void HandleOnMouseDown()
    {
        _isDragging = true;
        rb.isKinematic = false;  // Bắt đầu chịu vật lý

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance;
        _prevMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _prevMousePos.y = transform.position.y;

        curGridPos = Vector2Int.RoundToInt((Vector2)transform.position - center);
        prevGridPos = curGridPos;
    }

    void HandleOnMouseDrag()
    {
        if (!_isDragging) return;

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance;

        _curMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _curMousePos.y = transform.position.y;

        if(_curMousePos != _prevMousePos)
        {
            _targetPos = _curMousePos;

            // ✅ Di chuyển theo vận tốc để có va chạm thật
            Vector3 dir = (_targetPos - transform.position);
            rb.linearVelocity = dir / Time.fixedDeltaTime;

            // Giới hạn tốc độ (tránh jitter hoặc xuyên do tốc độ cao)
            if (rb.linearVelocity.magnitude > 15f)
                rb.linearVelocity = rb.linearVelocity.normalized * 15f;

            curGridPos = Vector2Int.RoundToInt(
                new Vector2(transform.position.x, transform.position.z)
                - center
                + BoardCtrl.I.Offset
            );

            BoardCtrl.I.Hover(curGridPos, _shapeData);
            if (curGridPos != prevGridPos)
            {
                prevGridPos = curGridPos;
            }
        }
    }

    void HandleOnMouseUp()
    {
        _isDragging = false;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;  // Ngừng vật lý

        Vector3 snapPos = BoardCtrl.I.GetSnapPosition(curGridPos, _shapeData);
        transform.position = snapPos;
        // Dọn hover
        BoardCtrl.I.UnHover();
    }

    public void ChangeColorByType()
    {
        shape.ChangeColorByType(data.colorBlock);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

[Serializable]
public class BlockWrapper
{
    //Block Property
    public int blockID;
    public ColorType colorBlock;
    public BlockDir blockDir;
    public int Rows { get; set; }
    public int Cols { get; set; }
}
