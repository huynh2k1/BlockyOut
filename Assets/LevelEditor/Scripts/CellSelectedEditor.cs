using GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectedEditor : MonoBehaviour
{
    public static CellSelectedEditor CurCellSelected;
    public ColorTypeSO colorTypeSO;
    public Image _img;

    public CellBoardType cellType;
    public ColorType colorType;

    private void Awake()
    {
        CurCellSelected = this;
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        cellType = CellBoardType.Field;
        colorType = ColorType.None;
    }

    public void LoadData(CellSelect cell)
    {
        cellType = cell.cellType;
        colorType = cell.colorType;
        _img.gameObject.SetActive(true);
        switch (cellType)
        {
            case CellBoardType.Field:
                _img.color = Color.white;
                break;
            case CellBoardType.Border:
                _img.color = colorTypeSO.ConvertColorTypeToColor(cell.colorType);
                break;
            case CellBoardType.Empty:
                _img.gameObject.SetActive(false);
                break;
        }

    }

}
