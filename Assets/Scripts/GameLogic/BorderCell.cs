using DG.Tweening;
using GameConfig;
using UnityEngine;

public class BorderCell : MonoBehaviour
{
    [SerializeField] ColorTypeSO colors;
    [SerializeField] Renderer _doorRenderer;
    [SerializeField] ParticleSystem _effectBroken;

    [SerializeField] GameObject _borderNormal;
    [SerializeField] GameObject _borderDoor;
    [SerializeField] GameObject plane;

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    public void PlayCrushingEffect()
    {
        _effectBroken.Play();

        _borderDoor.transform.DOKill();
        _borderDoor.transform.DOLocalMoveY(0.4f, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        _borderDoor.transform.DOLocalRotate(new Vector3(720f, 90f, 90f), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }

    public void ShowBorderByColorType(ColorType type)
    {
        bool isShow = (type == ColorType.None);
        _borderNormal.SetActive(isShow);
        plane.SetActive(!isShow);
        _borderDoor.SetActive(!isShow);
        if(type != ColorType.None)
        {
            ChangeColorByType(type);
        }
    }

    public void ChangeColorByType(ColorType type)
    {
        ChangeColor(colors.ConvertColorTypeToColor(type));
    }

    void ChangeColor(Color color)
    {
        if (_doorRenderer != null)
        {
            _doorRenderer.material.color = color;
        }
        if(_effectBroken != null)
        {
            var rend = _effectBroken.GetComponent<ParticleSystemRenderer>();
            rend.material = new Material(rend.sharedMaterial); // clone material
            rend.material.color = color;
        }
    }

    public void SetRotation(int index)
    {
        switch (index)
        {
            case 0:         //Left
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case 1:         //Bottom
                transform.rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
                break;
            case 2:         //Top
                transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                break;
            case 3:         //Right
                transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                break;
        }
    }
}
