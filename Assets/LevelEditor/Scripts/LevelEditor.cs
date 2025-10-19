using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using GameConfig;

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {

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

        //Grid
        [SerializeField] Transform _gridTransform;
        [SerializeField] List<CellEditor> listCell = new List<CellEditor>();
        private CellEditor[,] grid;
        [SerializeField] CellEditor _cellPrefab;

        //Block 
        [SerializeField] ShapeData[] _shapeDatas;

        private void Awake()
        {
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
            InitGrid();
        }
        #endregion

        #region BOARD GENERATE
        public void InitGrid()
        {
            CleadBoard();

            grid = new CellEditor[_rows, _columns];

            for (var r = 0; r < _rows; ++r)
            {
                for (var c = 0; c < _columns; ++c)
                {
                    var pos = GetCellPosition(r, c, _rows, _columns);
                    grid[r, c] = Instantiate(_cellPrefab, pos, Quaternion.identity, _gridTransform);
                    grid[r, c].row = r;
                    grid[r, c].col = r;
                    grid[r, c].name = $"( {r}, {c} )";
                    listCell.Add(grid[r, c]);

                    if (r == 0 || c == 0 || r == _rows - 1 || c == _columns - 1)
                    {
                        grid[r, c].UpdateUI(CellBoardType.Border);
                    }
                    else
                    {
                        grid[r, c].UpdateUI(CellBoardType.Field);
                    }
                }
            }
        }

        public void CleadBoard()
        {
            foreach(var cell in listCell)
            {
                cell.Destroy();
            }
        }

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

        private Vector3 GetCellPosition(int row, int col, int Rows, int Cols)
        {
            float offsetX = col - (Cols - 1) / 2f;
            float offsetZ = row - (Rows - 1) / 2f;
            return new Vector3(offsetX, 0, offsetZ);
        }
        #endregion

        #region BLOCK GENERATE
        public void GenerateBlocks()
        {
            for(var i = 0; i < _shapeDatas.Length; ++i)
            {

            }
        }
        #endregion
    }
}
