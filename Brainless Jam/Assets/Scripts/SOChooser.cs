using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SOChooser : MonoBehaviour
{
    public BlockScriptableObjects so;
    public BuilderUI builderUI;
    public int amount;
    public int cost;
    TextMeshProUGUI text;
    GameObject mouseblock;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = amount.ToString();
        cost = so.cost;
    }
    public void getSO()
    {
        if (builderUI.selectedBlock != so && builderUI.selectedBlock == null)
        {
            if (amount > 0)
            {
                builderUI.selectedBlock = so;
                builderUI.UpdateAmount(this.gameObject, -1);
                mouseblock = Instantiate(builderUI.mouseBlockPrefab, transform.parent.parent.parent.parent.parent.parent);
                mouseblock.GetComponent<Image>().sprite = so.sprite;
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

    public void BuyBlock()
    {
        if (PuzzlePieceManager.puzzlePieces >= cost)
        {
            builderUI.UpdateAmount(this.gameObject, 1);
            PuzzlePieceManager.puzzlePieces -= cost;
            text.text = amount.ToString();
        }
    }
}
