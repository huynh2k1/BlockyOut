using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using GameConfig;
using System.IO;

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
    public DataAllLevel dataAllLevel = new DataAllLevel();
    int _levelID;
    int _rows;
    int _columns;
    float _time;


    private void Awake()
    {
        I = this;
        _btnInitGrid.onClick.AddListener(OnClickInitGrid);
        _btnSave.onClick.AddListener(OnClickSave);
    }

    private void Start()
    {
        _filePath = Path.Combine(Application.dataPath, "_Data", "Levels.json");

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

    void OnClickSave()
    {
        if (!int.TryParse(_levelInputField.text, out _levelID) || _levelID < 0)
        {
            Debug.LogError("⚠️ Level is not valid");
            return;
        }
        if (!int.TryParse(_rowsInputField.text, out _rows) || _rows <= 0)
        {
            Debug.LogError("⚠️ Rows is not valid");
            return;
        }
        if (!int.TryParse(_columnsInputField.text, out _columns) || _columns <= 0)
        {
            Debug.LogError("⚠️ Columns is not valid");
            return;
        }
        if (!float.TryParse(_timeInputField.text, out _time) || _time < 0)
        {
            Debug.LogError("⚠️ Time is not valid");
            return;
        }

        SaveData();
    }
    #endregion

    #region BOARD Data
    

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

    #region SAVE LOAD LOGIC
    void SaveData()
    {
        // Đường dẫn tới thư mục Data All Level
        if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

        // ✅ Load data cũ trước
        if (File.Exists(_filePath))
        {
            string oldJson = File.ReadAllText(_filePath);
            dataAllLevel = JsonUtility.FromJson<DataAllLevel>(oldJson);
        }
        else
        {
            dataAllLevel = new DataAllLevel();
        }

        LevelData levelData = new LevelData
        {
            ID = _levelID,
            Rows = _rows,
            Cols = _columns,
            Time = _time,
            Cells = new CellData[board.listCell.Count],
            Blocks = new BlockData[board.listBlock.Count]

        };

        for (int i = 0; i < board.listCell.Count; i++)
        {
            CellData cell = new CellData
            {
                cellType = board.listCell[i].cellType,
                colorType = board.listCell[i].colorType,
            };
            levelData.Cells[i] = cell;
        }

        for(var i = 0; i < board.listBlock.Count; i++)
        {
            BlockWrapper data = board.listBlock[i].data;
            Vector3 pos = board.listBlock[i].Position;
            BlockData block = new BlockData
            {
                id = data.blockID,
                position = new TransformData(pos),
                blockDir = data.blockDir,
                colorType = data.colorBlock
            };
            levelData.Blocks[i] = block;
        }

        AddOrUpdateLevel(levelData);

        string json = JsonUtility.ToJson(dataAllLevel, true);
        File.WriteAllText(_filePath, json);

#if UNITY_EDITOR
        // Refresh Project window để thấy file ngay trong Unity Editor
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log($"✅ Saved level {_levelID} in: {_filePath}");
    }
    public void AddOrUpdateLevel(LevelData newLevel)
    {
        int index = dataAllLevel.Levels.FindIndex(l => l.ID == newLevel.ID);

        if (index >= 0)
        {
            // Nếu tồn tại ID, thì ghi đè
            dataAllLevel.Levels[index] = newLevel;
            Debug.Log($"✅ Level {newLevel.ID} đã được cập nhật.");
        }
        else
        {
            // Nếu chưa có thì thêm mới
            dataAllLevel.Levels.Add(newLevel);
            Debug.Log($"✅ Level {newLevel.ID} đã được thêm mới.");
        }
    }
    #endregion
}
