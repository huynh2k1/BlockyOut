using GameConfig;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CellBoard : MonoBehaviour
{
    [SerializeField] GameObject field;
    public BorderCell border;

    public int Row;
    public int Col;

    public void Initialize(CellBoardType type, ColorType colorType)
    {
        switch (type)
        {
            case CellBoardType.Field:
                FieldCell();
                break;
            case CellBoardType.Border:
                border.ShowBorderByColorType(colorType);
                BorderCell();
                break;
            case CellBoardType.Empty:
                EmptyCell();
                break;
        }
    }

    public void FieldCell()
    {
        field.SetActive(true);
        border.Show(false);
    }

    public void BorderCell()
    {
        field.SetActive(false);
        border.Show(true);
    }

    public void EmptyCell()
    {
        gameObject.SetActive(false);
    }
}
