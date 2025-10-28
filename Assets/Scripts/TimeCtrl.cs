using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeCtrl : MonoBehaviour
{
    public static TimeCtrl I;
    [SerializeField] Text timeText;
    [SerializeField] GameObject timePauseObj;
    public float TimeCD { get; set; }

    bool _isTiming;
    bool _isPaused; // NEW

    private void Awake()
    {
        I = this;
        ResetTimer();
    }

    public void SetTime(float time)
    {
        TimeCD = time;
        UpdateTimeText(TimeCD);
        _isTiming = true;
    }

    private void Update()
    {
        if (GameCtrl.I.CurState != GameConfig.GameState.Playing)
            return;
        if (_isTiming == false)
            return;
        if (_isPaused) // NEW
            return;

        TimeCD -= Time.deltaTime;
        UpdateTimeText(TimeCD);

        if (TimeCD <= 0)
        {
            TimeCD = 0;
            _isTiming = false;
            GameCtrl.I.GameLose();
        }
    }

    public void PauseTimer(float pauseDuration, Action actionDone = default)
    {
        if (_isPaused)
            return;
        ShowTimePauseObj(true);
        StartCoroutine(PauseCoroutine(pauseDuration, actionDone));
    }

    private IEnumerator PauseCoroutine(float duration, Action actionDone = default)
    {
        _isPaused = true;
        yield return new WaitForSeconds(duration);
        _isPaused = false;
        ShowTimePauseObj(false);
        actionDone?.Invoke();
    }

    public void ResetTimer()
    {
        ShowTimePauseObj(false);
        _isTiming = false;
        _isPaused = false;
    }

    public void UpdateTimeText(float currentTime)
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = $"{minutes:00} : {seconds:00}";
    }

    public void ShowTimePauseObj(bool isShow)
    {
        //timePauseObj.SetActive(isShow);
    }
}
