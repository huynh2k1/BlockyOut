using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;

    public event Action OnCellClickedEvent;

    private void OnMouseDown()
    {
        OnCellClickedEvent?.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true); 
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

