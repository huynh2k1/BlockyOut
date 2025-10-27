using UnityEngine;
using GameConfig;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DG.Tweening;
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

    public List<Vector2Int> occupiedPoints; //Những ô trên grid mà block đang chiếm đóng


    public Vector3 GetPosition => transform.position;   

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;              
        // Bình thường không chịu vật lý
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Initialize(Vector3 pos, int id, ColorType colorType, BlockDir blockDir, ShapeData shapeData)
    {
        transform.position = pos;
        _shapeData = shapeData; 
        shape = Instantiate(shapeData.shapePrefab, transform);
        center = new Vector2(shapeData.columns / 2f, shapeData.rows / 2f);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, shapeData.rotation.z, transform.rotation.z));

        shape.HandleOnMouseDown(HandleOnMouseDown);
        shape.HandleOnMouseDrag(HandleOnMouseDrag);
        shape.HandleOnMouseUp(HandleOnMouseUp);

        data.Rows = shapeData.rows;
        data.Cols = shapeData.columns;
        data.blockID = id;
        data.colorBlock = colorType;    
        data.blockDir = blockDir;

        ChangeColorByType();

        //Hover Points
        curGridPos = Vector2Int.RoundToInt(
                new Vector2(transform.position.x, transform.position.z)
                - center
                + BoardCtrl.I.Offset
            );


        PlaceBlock();
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

        curGridPos = Vector2Int.RoundToInt(
                new Vector2(transform.position.x, transform.position.z)
                - center
                + BoardCtrl.I.Offset
            );
        prevGridPos = curGridPos;

        ClearOccupiedPoint();
    }

    void HandleOnMouseDrag()
    {
        if (shape.isOuting)
            return;
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
            if (rb.linearVelocity.magnitude > 10f)
                rb.linearVelocity = rb.linearVelocity.normalized * 10f;


            curGridPos = Vector2Int.RoundToInt(
                new Vector2(transform.position.x, transform.position.z)
                - center
                + BoardCtrl.I.Offset
            );

            if (curGridPos != prevGridPos)
            {
                BoardCtrl.I.Hover(curGridPos, _shapeData);
                BoardCtrl.I.CanExit(this);
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
        PlaceBlock();
    }

    public void StartOut()
    {
        rb.isKinematic = true;  // Bắt đầu chịu vật lý

        ClearOccupiedPoint();
        BoardCtrl.I.UnHover();
        Vector3 snapPos = BoardCtrl.I.GetSnapPosition(curGridPos, _shapeData);
        transform.position = snapPos;
    }

    public void BlockOut(Vector3 target, Action actionDone = default)
    {
        shape.isOuting = true;
        
        transform.DOKill();
        transform.DOMove(target, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            actionDone?.Invoke();

            Destroy();
        });
    }

    public void ChangeColorByType()
    {
        shape.ChangeColorByType(data.colorBlock);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void PlaceBlock()
    {
        ClearOccupiedPoint();
        var points = BoardCtrl.I.GetOccupiedPoints(curGridPos, _shapeData);
        occupiedPoints = new List<Vector2Int>(points);
        BoardCtrl.I.PlaceBlock(curGridPos, _shapeData);
    }

    public void ClearOccupiedPoint()
    {
        BoardCtrl.I.ClearDataPoint(occupiedPoints);
        occupiedPoints.Clear(); 
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
