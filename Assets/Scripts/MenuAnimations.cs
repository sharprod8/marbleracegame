using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimations : MonoBehaviour
{
    public TMP_Text titleText;
    public RectTransform menuPanel;

    public Button normalButton;
    public Button speedrunButton;
    public Button modMayhemButton;

    private void Start()
    {
        normalButton.transform.localScale = Vector3.zero;
        speedrunButton.transform.localScale = Vector3.zero;
        modMayhemButton.transform.localScale = Vector3.zero;

        LeanTween.scale(normalButton.gameObject, Vector3.one, 0.4f).setDelay(0.2f).setEaseOutBack();
        LeanTween.scale(speedrunButton.gameObject, Vector3.one, 0.4f).setDelay(0.4f).setEaseOutBack();
        LeanTween.scale(modMayhemButton.gameObject, Vector3.one, 0.4f).setDelay(0.6f).setEaseOutBack();
        LeanTween.moveLocalY(menuPanel.gameObject, menuPanel.localPosition.y + 10f, 2f).setEaseInOutSine().setLoopPingPong();
    }
}