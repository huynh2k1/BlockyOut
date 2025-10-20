using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using GameConfig;

public class LevelEditorCtrl : MonoBehaviour
{
    public static LevelEditorCtrl I;

    public BoardEditor board;
    public MenuCtrl menuCtrl;
    string _filePath;

    //Button
    [SerializeField] Button _btnInitGrid;
    [SerializeField] Button _btnEditLevel;
    [SerializeField] Button _btnSave;

    //InputField
    [SerializeField] InputField _levelInputField;
    [SerializeField] InputField _rowsInputField;
    [SerializeField] InputField _columnsInputField;
    [SerializeField] InputField _timeInputField;

    //Level Data
    int _levelID;
    int _rows;
    int _columns;
    int _move;


    private void Awake()
    {
        I = this;
        _btnInitGrid.onClick.AddListener(OnClickInitGrid);
    }

    private void Start()
    {
        menuCtrl.InitMenu();
    }

    #region ONLICK EVENTS
    void OnClickInitGrid()
    {
        GetRowsFromInput();
        GetColsFromInput();

        if (_rows <= 0 || _columns <= 0)
        {
            Debug.LogError("⚠️ Rows And Columns Not Valid");
            return;
        }
        board.InitGrid(_rows, _columns);
    }
    #endregion

    #region BOARD GENERATE
        

    void GetRowsFromInput()
    {
        _rows = ParseInputToInt(_rowsInputField);
    }

    void GetColsFromInput()
    {
        _columns = ParseInputToInt(_columnsInputField);
    }

    int ParseInputToInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            Debug.LogWarning("Giá trị nhập không hợp lệ!");
            value = 0;
        }
        return value;
    }

    #endregion

        
}
