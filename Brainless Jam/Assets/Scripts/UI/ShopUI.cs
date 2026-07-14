using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour
{
    public GameObject[] blockSlots;
    public BlockScriptableObjects[] allBlocks;
    public TextMeshProUGUI puzzlePiecesText;
    HashSet<int> randomBlocks = new HashSet<int>();

    private void Start()
    {
        foreach(var slot in blockSlots)
        {
            int randomBlock = 0;
            ShopBlock script = slot.GetComponent<ShopBlock>();
            while (randomBlocks.Contains(randomBlock))
            {
                randomBlock = Random.Range(0, allBlocks.Length);
            }
            if (!randomBlocks.Contains(randomBlock))
            {
                randomBlocks.Add(randomBlock);
            }
            script.SO = allBlocks[randomBlock];
            script.InitiateShop();
        }

        puzzlePiecesText.text = PuzzlePieceManager.puzzlePieces.ToString();
    }

    public void BuyBlock()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        ShopBlock buttonScript = button.GetComponent<ShopBlock>();
        if (PuzzlePieceManager.puzzlePieces >= buttonScript.SO.cost)
        {
            TowerParts.Instance.AddTower(buttonScript.SO.name, 1);
            PuzzlePieceManager.puzzlePieces -= buttonScript.SO.cost;
            puzzlePiecesText.text = PuzzlePieceManager.puzzlePieces.ToString();
        }
    }
}
