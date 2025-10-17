using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;

    public void Active(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}