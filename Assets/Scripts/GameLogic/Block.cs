using UnityEngine;
using GameConfig;
using UnityEngine.UI;
public class Block : MonoBehaviour
{
    public BlockWrapper data;
    private Shape shape;

    Vector3 inputDelta;
    
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

    }

    void HandleOnMouseDrag()
    {

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

public struct BlockWrapper
{
    //Block Property
    public int blockID;
    public ColorType colorBlock;
    public BlockDir blockDir;
    public int Rows { get; set; }
    public int Cols { get; set; }
}
