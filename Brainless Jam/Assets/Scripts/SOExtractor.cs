using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SOExtractor : MonoBehaviour
{
    public BlockScriptableObjects blockSO;
    SpriteRenderer sr;
    Image image;

    public bool wasExtracted;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (blockSO != null && !wasExtracted)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = blockSO.sprite;
                
            }
            else
            {
                image = GetComponent<Image>();
                image.sprite = blockSO.sprite;
                float aspectRatio = (float)image.sprite.texture.width / image.sprite.texture.height;

                image.GetComponent<RectTransform>().sizeDelta = new Vector2(160 * aspectRatio, 160);
            }

            name = blockSO.nameObject;
            wasExtracted = true;
        }
    }
}
