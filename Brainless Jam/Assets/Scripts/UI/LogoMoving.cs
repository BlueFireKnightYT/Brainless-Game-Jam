using UnityEngine;

public class LogoMoving : MonoBehaviour
{
    public float moveAmount = 10f;      // Pixels
    public float moveSpeed = 1f;

    public float rotateAmount = 5f;     // Degrees
    public float rotateSpeed = 1.2f;

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Move up and down
        float y = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        rectTransform.anchoredPosition = startPos + new Vector2(0, y);

        // Rotate back and forth
        float angle = Mathf.Sin(Time.time * rotateSpeed) * rotateAmount;
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
