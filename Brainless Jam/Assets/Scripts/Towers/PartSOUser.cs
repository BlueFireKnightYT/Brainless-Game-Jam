using UnityEngine;

public class PartSOUser : MonoBehaviour
{
    public BlockScriptableObjects partSO;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = partSO.sprite;
    }
}
