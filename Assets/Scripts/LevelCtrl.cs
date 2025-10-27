using UnityEngine;
using System.Linq;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] BoardCtrl boardCtrl;
    [SerializeField] TextAsset _dataAllLevel;
    private DataAllLevel _levelData;


    private void Awake()
    {
        LoadLevelsFromTextAsset();  
    }

    #region CONVERT JSON TO DATA
    private void LoadLevelsFromTextAsset()
    {
        if (_dataAllLevel == null)
        {
            Debug.LogError("⚠️ Chưa gán file Levels.json vào LevelLoader!");
            return;
        }

        _levelData = JsonUtility.FromJson<DataAllLevel>(_dataAllLevel.text);
    }

    public LevelData GetLevelByID(int levelID)
    {
        if (_levelData == null || _levelData.Levels == null)
        {
            Debug.LogError("❌ Dữ liệu level chưa được load!");
            return null;
        }

        return _levelData.Levels.FirstOrDefault(l => l.ID == levelID);
    }
    #endregion

    public void OnLevelStart()
    {
        LevelData data = GetLevelByID(PrefData.CurLevel);
        boardCtrl.Initialize(data);
    }

    public void OnNextLevel()
    {
        if(PrefData.CurLevel < _levelData.Levels.Count - 1)
            PrefData.CurLevel++;
        else
            PrefData.CurLevel = 0;

        LevelData data = GetLevelByID(PrefData.CurLevel);
        boardCtrl.Initialize(data);
    }

    public void OnLevelLose()
    {

    }
}
