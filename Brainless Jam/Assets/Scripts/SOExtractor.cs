using UnityEngine;

public class SOExtractor : MonoBehaviour
{
    public BlockScriptableObjects blockSO;
    SpriteRenderer sr;

    bool wasExtracted;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blockSO != null && !wasExtracted)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = blockSO.sprite;
            name = blockSO.nameObject;
        }
    }
}
