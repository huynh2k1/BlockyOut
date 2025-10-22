using GameConfig;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BlockModel : MonoBehaviour
{
    //[SerializeField] BlocksVisualSO data;
    [SerializeField] ColorTypeSO colors;

    public MeshFilter meshFilter;

    public Renderer renderer;

    public bool IsMeshValid(ShapeData shapeData)
    {
        if (shapeData.meshData == null)
            return false;

        return true;
    }

    public void ChangeMesh(ShapeData shapeData)
    {
        meshFilter.mesh = shapeData.meshData;
        transform.rotation = Quaternion.Euler(new Vector3(shapeData.rotation.x, shapeData.rotation.z, shapeData.rotation.y));
    }

    public void ChangeColorByType(ColorType type)
    {
        ChangeColor(colors.ConvertColorTypeToColor(type));
    }

    public void ChangeColor(Color color)
    {
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
