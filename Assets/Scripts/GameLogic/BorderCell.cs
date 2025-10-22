using GameConfig;
using UnityEngine;

public class BorderCell : MonoBehaviour
{
    [SerializeField] ColorTypeSO colors;
    [SerializeField] Renderer _doorRenderer;

    [SerializeField] GameObject _borderNone;
    [SerializeField] GameObject _borderDoor;
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    public void ShowBorderByColorType(ColorType type)
    {
        bool isShow = (type == ColorType.None);
        _borderNone.SetActive(isShow);
        _borderDoor.SetActive(!isShow);
        if(type != ColorType.None)
        {
            ChangeColorByType(type);
        }
    }

    public void ChangeColorByType(ColorType type)
    {
        ChangeColor(colors.ConvertColorTypeToColor(type));
    }

    void ChangeColor(Color color)
    {
        if (_doorRenderer != null)
        {
            _doorRenderer.material.color = color;
        }
    }

    public void SetRotation(int index)
    {
        switch (index)
        {
            case 0:         //Left
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case 1:         //Bottom
                transform.rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
                break;
            case 2:         //Top
                transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                break;
            case 3:         //Right
                transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                break;
        }
    }
}
