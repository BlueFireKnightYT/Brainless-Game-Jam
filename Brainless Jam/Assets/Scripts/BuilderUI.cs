using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    public BlockScriptableObjects selectedBlock;
    public BlockScriptableObjects emptyBlock;

    public GameObject[] allBlocks;
    public GameObject[] gridBlocks;
    public BlockScriptableObjects[] towerToSpawn;
    public GameObject towerTemplate;

    public GameObject buildUI;

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

    public void FinishBuild()
    {
        bool notEmpty = false;
        for(int i = 0; i < gridBlocks.Length; i++)
        {
            BlockScriptableObjects gridBlockSO = gridBlocks[i].GetComponentInChildren<SOExtractor>().blockSO;
            if (gridBlockSO != emptyBlock && gridBlockSO != null)
            {
                towerToSpawn[i] = gridBlockSO;
                notEmpty = true;
            }
            else
            {
                towerToSpawn[i] = null;
            }
        }

        if(notEmpty)
        {
            GameObject currentTower = Instantiate(towerTemplate);
            currentTower.GetComponent<TowerInitialiser>().towerParts = towerToSpawn;
            currentTower.GetComponent<TowerInitialiser>().BuildTower();
            notEmpty = false;
            buildUI.SetActive(false);
        }
    }
}
