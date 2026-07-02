using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    public BlockScriptableObjects selectedBlock;
    public BlockScriptableObjects emptyBlock;

    public GameObject[] allBlocks;

    public void ChooseGrid()
    {
        if (selectedBlock != null)
        { 
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            SOExtractor soExtractor = selectedButton.GetComponentInChildren<SOExtractor>();
            soExtractor.blockSO = selectedBlock;
            soExtractor.wasExtracted = false;
            selectedBlock = null;
        }
        else
        {
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            SOExtractor soExtractor = selectedButton.GetComponentInChildren<SOExtractor>();
            BlockScriptableObjects lastBlock = soExtractor.blockSO;
            soExtractor.blockSO = emptyBlock;
            soExtractor.wasExtracted = false;
            foreach (GameObject block in allBlocks)
            {
                if(block.GetComponent<SOChooser>().so == lastBlock)
                {
                    UpdateAmount(block, 1);
                    return;
                }
            }
        }
    }

    public void UpdateAmount(GameObject block, int amount)
    {
        block.GetComponent<SOChooser>().amount+= amount;
        block.GetComponentInChildren<TextMeshProUGUI>().text = block.GetComponent<SOChooser>().amount.ToString();
        if (block.GetComponent<SOChooser>().amount <= 0)
        {
            Image[] images = block.GetComponentsInChildren<Image>();

            foreach (Image img in images)
            {
                if (img.gameObject != block) 
                {
                    img.color = Color.gray8;
                    break;
                }
            }
        }
        else
        {
            Image[] images = block.GetComponentsInChildren<Image>();

            foreach (Image img in images)
            {
                if (img.gameObject != block)
                {
                    img.color = Color.white;
                    break;
                }
            }

        }
    }
}
