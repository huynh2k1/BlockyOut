using UnityEngine;
using GameConfig;
using UnityEngine.UI;
using System;
public class Block : MonoBehaviour
{
    Rigidbody rb;
    public BlockWrapper data;
    private Shape shape;

    Vector3 _prevMousePos;
    Vector3 _curMousePos;
    Vector3 _targetPos;

    Vector2Int previousDragPoint;
    Vector2Int currentDragPoint;

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void Initialize(int id, ColorType colorType, BlockDir blockDir,ShapeData shapeData)
    {
        shape = Instantiate(shapeData.shapePrefab, transform);
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
        Debug.Log("DOWN");
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 quan trọng


        _prevMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _prevMousePos.y = 0;

        //currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - )
    }

    void HandleOnMouseDrag()
    {
        Debug.Log("DRAG");
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance; // 👈 thêm dòng này khi drag

        _curMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _curMousePos.y = 0;

        //if(_curMousePos != _prevMousePos)
        //{
        //    _prevMousePos = _curMousePos;
        //}

        _targetPos = _curMousePos;
        rb.MovePosition(_targetPos);
    }

    void HandleOnMouseUp()
    {

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
