using GameConfig;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private static Shape currentShapeDragging;

    [SerializeField] ColorTypeSO colors;
    public Renderer _renderer;

    private event Action OnMouseDownEvent;
    private event Action OnMouseDragEvent;
    private event Action OnMouseUpEvent;

    public bool isOuting { get; set; }

    void Update()
    {
        if (isOuting) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    currentShapeDragging = this;
                    OnMouseDownEvent?.Invoke();
                }
            }
        }

        

        if (Input.GetMouseButtonUp(0))
        {
            if (currentShapeDragging == this)
            {
                OnMouseUpEvent?.Invoke();
                currentShapeDragging = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isOuting) return;
        if (Input.GetMouseButton(0))
        {
            if (currentShapeDragging == this)
                OnMouseDragEvent?.Invoke();
        }
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
