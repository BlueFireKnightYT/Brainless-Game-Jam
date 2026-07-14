using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBlock : MonoBehaviour
{
    public BlockScriptableObjects SO;
    public Image image;
    TextMeshProUGUI costText;
    int cost;

    private void Awake()
    {
        image = GetComponent<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void InitiateShop()
    {
        image.sprite = SO.sprite;
        float aspectRatio = (float)image.sprite.texture.width / image.sprite.texture.height;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(125 * aspectRatio, 125);
        cost = SO.cost;
        costText.text = ("%" + cost.ToString());
    }
}
