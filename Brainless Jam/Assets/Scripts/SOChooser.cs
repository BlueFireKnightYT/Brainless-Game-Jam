using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SOChooser : MonoBehaviour
{
    public BlockScriptableObjects so;
    public BuilderUI builderUI;
    public int amount;
    TextMeshProUGUI text;
    GameObject mouseblock;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = amount.ToString();
    }
    public void getSO()
    {
        if (builderUI.selectedBlock != so && builderUI.selectedBlock == null)
        {
            if (amount > 0)
            {
                builderUI.selectedBlock = so;
                builderUI.UpdateAmount(this.gameObject, -1);
                mouseblock = Instantiate(builderUI.mouseBlockPrefab, transform.parent.parent);
                mouseblock.GetComponent<Image>().sprite = so.sprite;
                builderUI.puzzlePieceAmount += so.piecesPerTime;
                builderUI.timePerEarn += so.timePerEarn;
                builderUI.currentMouseBlock = mouseblock;
            }
        }
        else if (builderUI.selectedBlock == so)
        {
            builderUI.UpdateAmount(this.gameObject, +1);
            builderUI.selectedBlock = null;
            if (mouseblock != null) Destroy(mouseblock);
        }
    }
}
