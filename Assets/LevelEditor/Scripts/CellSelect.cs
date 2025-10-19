using GameConfig;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellSelect : MonoBehaviour, IPointerDownHandler
{
    public Image _img;
    public CellBoardType cellType;
    public ColorTypeSO colorTypeSO;
    public ColorType colorType;

    public void OnPointerDown(PointerEventData eventData)
    {
        CellSelectedEditor.CurCellSelected.LoadData(this);
    }

    public void LoadData(CellBoardType type, ColorType colorType = ColorType.None)
    {
        _img.gameObject.SetActive(true);
        cellType = type;
        this.colorType = colorType;

        switch (type)
        {
            case CellBoardType.Field:
                _img.color = Color.white;
                break;
            case CellBoardType.Border:
                _img.color = colorTypeSO.ConvertColorTypeToColor(colorType);
                break;
            case CellBoardType.Empty:
                _img.gameObject.SetActive(false);
                break;
        }
    }

}
