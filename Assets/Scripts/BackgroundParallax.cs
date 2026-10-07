using UnityEngine;
using UnityEngine.UI;

public class CheckerScroll : MonoBehaviour
{
    public float speed = 0.1f;
    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }

    private void Update()
    {
        image.uvRect = new Rect(Time.time * speed, 0, 1, 1);
    }
}