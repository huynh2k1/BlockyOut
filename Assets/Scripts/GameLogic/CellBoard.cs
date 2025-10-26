using GameConfig;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CellBoard : MonoBehaviour
{
    public CellBoardType cellType;
    public ColorType colorType;
    [SerializeField] GameObject field;
    [SerializeField] GameObject hover;
    public BorderCell border;

    public int Row;
    public int Col;

    public void Initialize(CellBoardType Type, ColorType ColorType)
    {
        cellType = Type;
        colorType = ColorType;
        switch (Type)
        {
            case CellBoardType.Field:
                FieldCell();
                break;
            case CellBoardType.Border:
                border.ShowBorderByColorType(ColorType);
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

    public void Hover(bool isHover)
    {
        hover.SetActive(isHover);

    }
}
