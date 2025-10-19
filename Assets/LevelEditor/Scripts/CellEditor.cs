using UnityEngine;
using GameConfig;
using JetBrains.Annotations;
namespace LevelEditor
{
    public class CellEditor : MonoBehaviour
    {
        public ColorTypeSO colorTypeSO;
        public int row;
        public int col;

        public CellBoardType cellType;
        public ColorType colorType;

        public GameObject fieldObject;
        public GameObject borderObject;
        public GameObject emptyObject;

        public SpriteRenderer _borderRenderer;

        private void OnMouseDown()
        {
            CellSelectedEditor data = CellSelectedEditor.CurCellSelected;
            Debug.Log($"{data.cellType.ToString()} {data.colorType.ToString()}");
            UpdateUI(data.cellType, data.colorType);
        }

        public void UpdateUI(CellBoardType value, ColorType colorType = ColorType.None)
        {
            fieldObject.SetActive(false);
            borderObject.SetActive(false);
            emptyObject.SetActive(false);

            cellType = value;
            this.colorType = colorType;

            switch (cellType)
            {
                case CellBoardType.Field:
                    fieldObject.SetActive(true);
                    break;
                case CellBoardType.Border:
                    borderObject.SetActive(true);
                    _borderRenderer.color = colorTypeSO.ConvertColorTypeToColor(colorType);
                    break;
                case CellBoardType.Empty:
                    emptyObject.SetActive(true);    
                    break;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        //private CellBoardType GetNextType(CellBoardType current)
        //{
        //    int next = (int)current + 1;
        //    int max = System.Enum.GetValues(typeof(CellBoardType)).Length;

        //    if (next >= max)
        //        next = 0;

        //    return (CellBoardType)next;
        //}
    }
}
