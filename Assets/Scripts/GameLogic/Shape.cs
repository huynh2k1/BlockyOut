using GameConfig;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] ColorTypeSO colors;
    public Renderer _renderer;

    private event Action OnMouseDownEvent;
    private event Action OnMouseDragEvent;
    private event Action OnMouseUpEvent;

    private void OnMouseDown()
    {
        OnMouseDownEvent?.Invoke();
    }

    private void OnMouseDrag()
    {
        OnMouseDragEvent?.Invoke();
    }
    private void OnMouseUp()
    {
        OnMouseUpEvent?.Invoke();
    }

    public void HandleOnMouseDown(Action action)
    {
        OnMouseDownEvent = action;
    }

    public void HandleOnMouseDrag(Action action)
    {
        OnMouseDragEvent = action;
    }

    public void HandleOnMouseUp(Action action)
    {
        OnMouseUpEvent = action;
    }

    public void ChangeColorByType(ColorType type)
    {
        ChangeColor(colors.ConvertColorTypeToColor(type));
    }

    public void ChangeColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.material.color = color;
        }
    }
}
