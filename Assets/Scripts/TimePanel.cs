using UnityEngine;
using UnityEngine.UI;

public class TimePanel : MonoBehaviour
{
    [SerializeField] Text timeText;

    [SerializeField] float time = 145;

    private void Start()
    {
        UpdateTimeText(time);
    }

    void UpdateTimeText(float currentTime)
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = $"{minutes:00} : {seconds:00}";
    }
}
