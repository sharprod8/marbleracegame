using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.15f).setEaseOutBack();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector3.one, 0.15f).setEaseOutBack();
    }
}