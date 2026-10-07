using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHoverReset : MonoBehaviour ,IPointerEnterHandler
{
    public ModePanel[] allPanels;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ModePanel.ResetPanels(allPanels);
    }
}