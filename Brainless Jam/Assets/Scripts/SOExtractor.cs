using UnityEngine;
using UnityEngine.UI;

public class SOExtractor : MonoBehaviour
{
    public BlockScriptableObjects blockSO;
    SpriteRenderer sr;
    Image image;

    public bool wasExtracted;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blockSO != null && !wasExtracted)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = blockSO.sprite;
            else
            {
                image = GetComponent<Image>();
                image.sprite = blockSO.sprite;
            }

            name = blockSO.nameObject;
            wasExtracted = true;
        }
    }
}
