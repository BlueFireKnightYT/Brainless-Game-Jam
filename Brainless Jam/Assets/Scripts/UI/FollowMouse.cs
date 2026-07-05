using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    Image image;
    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        transform.position = mouseWorldPos;
    }

    private void Start()
    {
        image = GetComponent<Image>();
        float aspectRatio = (float)image.sprite.texture.width / image.sprite.texture.height;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(160 * aspectRatio, 160);
    }
}
