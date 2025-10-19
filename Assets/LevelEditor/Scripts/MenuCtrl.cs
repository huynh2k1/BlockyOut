using UnityEngine;
using GameConfig;
public class MenuCtrl : MonoBehaviour
{

    public Transform parentCell;
    public CellSelect cellPrefab;



    public void InitMenu()
    {
        //Init Cell Field 
        CellSelect cellField = Instantiate(cellPrefab, parentCell);
        cellField.LoadData(CellBoardType.Field);
        cellField.name = $"Cell {CellBoardType.Field.ToString()}";

        ////Init Cell Empty
        CellSelect cellEmpty = Instantiate(cellPrefab, parentCell);
        cellEmpty.LoadData(CellBoardType.Empty);
        cellEmpty.name = $"Cell {CellBoardType.Empty.ToString()}";


        //Init Cells Border
        foreach (ColorType color in System.Enum.GetValues(typeof(ColorType)))
        {
            CellSelect cellBorder = Instantiate(cellPrefab, parentCell);
            cellBorder.LoadData(CellBoardType.Border, color);
            cellBorder.name = $"{CellBoardType.Border.ToString()} {color.ToString()}";
        }
    }
}
