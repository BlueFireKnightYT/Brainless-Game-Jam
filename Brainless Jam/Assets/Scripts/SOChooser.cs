using TMPro;
using UnityEngine;

public class SOChooser : MonoBehaviour
{
    public BlockScriptableObjects so;
    public BuilderUI builderUI;
    public int amount;
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = amount.ToString();
    }
    public void getSO()
    {
        if (amount > 0)
        {
            builderUI.selectedBlock = so;
            builderUI.UpdateAmount(this.gameObject, -1);
        }  
    }
}
