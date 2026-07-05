using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    public BlockScriptableObjects selectedBlock;
    public BlockScriptableObjects emptyBlock;

    public GameObject[] allBlocks;
    public GameObject[] gridBlocks;
    public BlockScriptableObjects[] towerToSpawn;
    public GameObject towerTemplate;

    public GameObject mouseBlockPrefab;
    public GameObject currentMouseBlock;

    public GameObject buildUI;

    public Quaternion[] storedPartRotations;
    public Vector3[] storedPartScales;

    public int puzzlePieceAmount;
    public int timePerEarn;
    public TextMeshProUGUI text;

    public void ChooseGrid()
    {
        if (selectedBlock != null)
        { 
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            SOExtractor soExtractor = selectedButton.GetComponentInChildren<SOExtractor>();
            soExtractor.blockSO = selectedBlock;
            soExtractor.wasExtracted = false;
            selectedButton.transform.localScale = currentMouseBlock.transform.localScale;
            selectedButton.transform.rotation = currentMouseBlock.transform.rotation;
            if(currentMouseBlock != null) Destroy(currentMouseBlock);
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
                if (block.GetComponent<SOChooser>().so == lastBlock)
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
                storedPartRotations[i] = gridBlocks[i].transform.rotation;
                storedPartScales[i] = gridBlocks[i].transform.localScale;
                notEmpty = true;
            }
            else
            {
                towerToSpawn[i] = null;
                storedPartRotations[i] = Quaternion.Euler(1, 1, 1);
                storedPartScales[i] = Vector3.zero;
            }
        }

        if(notEmpty)
        {
            GameObject currentTower = Instantiate(towerTemplate);
            TowerInitialiser tI = currentTower.GetComponent<TowerInitialiser>();
            tI.towerParts = towerToSpawn;
            tI.towerPartsRotations = storedPartRotations;
            tI.towerPartsScales = storedPartScales;
            tI.puzzlePieceAmount = puzzlePieceAmount;
            tI.timePerEarn = timePerEarn;
            notEmpty = false;
            for (int i = 0; i < gridBlocks.Length; i++)
            {
                if (gridBlocks[i].GetComponentInChildren<SOExtractor>().blockSO == null)
                    continue;
                gridBlocks[i].GetComponentInChildren<SOExtractor>().blockSO = null;
                gridBlocks[i].transform.GetChild(0).GetComponent<Image>().sprite = emptyBlock.sprite;
                gridBlocks[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                gridBlocks[i].transform.localScale = Vector3.one;
                gridBlocks[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            buildUI.SetActive(false);
        }
        puzzlePieceAmount = 0;
        timePerEarn = 0;
    }

    private void Update()
    {
        if(Mouse.current.rightButton.wasPressedThisFrame && selectedBlock != null)
        {
            currentMouseBlock.transform.rotation *= Quaternion.Euler(0, 0, 90);
            if (currentMouseBlock.transform.eulerAngles.z == 180f || currentMouseBlock.transform.eulerAngles.z == 0f)
            {
                currentMouseBlock.transform.localScale = new Vector3(currentMouseBlock.transform.localScale.x, currentMouseBlock.transform.localScale.y * -1, currentMouseBlock.transform.localScale.z);
            }
        }

        text.text = PuzzlePieceManager.puzzlePieces.ToString();
    }
}
