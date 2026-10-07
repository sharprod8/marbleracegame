using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject p1Button;
    public GameObject p2Button;

    public ModePanel[] allPanels;

    private Vector3 normalScale = Vector3.one;
    private Vector3 expandedScale = new Vector3(1.1f, 1.1f, 1f);
    private Vector3 shrunkScale = new Vector3(0.9f, 0.9f, 1f);

    private Vector3 startPos;
    private Image panelImage;

    private void Start()
    {
        startPos = transform.localPosition;

        p1Button.SetActive(false);
        p2Button.SetActive(false);
    }

    private void Awake()
    {
        panelImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (ModePanel panel in allPanels)
        {
            LeanTween.cancel(panel.gameObject);

            if (panel == this)
            {
                LeanTween.scale(panel.gameObject, expandedScale, 0.25f).setEaseOutBack();
                //LeanTween.moveLocalY(panel.gameObject, panel.startPos.y - 30f, 0.25f).setEaseOutBack();

                //panel.panelImage.color = new Color(0.9f, 0.9f, 0.9f, 1f);

                panel.p1Button.SetActive(true);
                panel.p2Button.SetActive(true);
            }
            else
            {
                LeanTween.scale(panel.gameObject, shrunkScale, 0.25f).setEaseOutBack();
                //LeanTween.moveLocalY(panel.gameObject, panel.startPos.y, 0.25f).setEaseOutBack();

                //panel.panelImage.color = new Color(0.85f, 0.85f, 0.85f, 1f);

                panel.p1Button.SetActive(false);
                panel.p2Button.SetActive(false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //nothin
    }

    public static void ResetPanels(ModePanel[] panels)
    {
        foreach (ModePanel panel in panels)
        {
            LeanTween.scale(panel.gameObject, Vector3.one, 0.2f).setEaseOutBack();
            //LeanTween.moveLocalY(panel.gameObject, panel.startPos.y, 0.2f).setEaseOutBack();

            panel.p1Button.SetActive(false);
            panel.p2Button.SetActive(false);
        }
    }
}