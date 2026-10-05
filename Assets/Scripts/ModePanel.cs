using UnityEngine;
using UnityEngine.EventSystems;

public class ModePanel : MonoBehaviour, IPointerEnterHandler
{
    public GameObject p1Button;
    public GameObject p2Button;

    public ModePanel[] allPanels;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (ModePanel panel in allPanels)
        {
            if (panel == this)
            {
                LeanTween.scale(panel.gameObject, new Vector3(1.25f, 1.25f, 1f), 0.25f).setEaseOutBack();
                panel.p1Button.SetActive(true);
                panel.p2Button.SetActive(true);
            }
            else
            {
                LeanTween.scale(panel.gameObject, new Vector3(0.8f, 0.8f, 1f), 0.25f).setEaseOutBack();
                panel.p1Button.SetActive(false);
                panel.p2Button.SetActive(false);
            }
        }
    }
}