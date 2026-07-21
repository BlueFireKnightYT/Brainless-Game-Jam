using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    [Header("Block SO's")]
    public BlockScriptableObjects selectedBlock;
    public BlockScriptableObjects emptyBlock;
    public GameObject originButton;

    [System.Serializable]
    public class Preset
    {
        public string presetName;
        public List<Vector2Int> parts = new List<Vector2Int>();
    }

    [Header("Presets")]
    public Preset[] presets;

    [Header("Text Components")]
    public TextMeshProUGUI synergyText;
    public TextMeshProUGUI layoutBonusText;

    public GameObject[] allBlocks;
    public GameObject[] gridBlocks;
    public BlockScriptableObjects[] towerToSpawn;
    public GameObject[] originButtons;
    public GameObject towerTemplate;

    public GameObject mouseBlockPrefab;
    public GameObject currentMouseBlock;

    [Header("UI Parts")]
    public GameObject buildUI;
    public GameObject removeUI;
    public GameObject moveUI;
    public GameObject lookUpSynergyUI;

    public bool isRemoving;
    public bool isMoving;

    public Quaternion[] storedPartRotations;
    public Vector3[] storedPartScales;

    public int puzzlePieceAmount;
    public int timePerEarn;
    public TextMeshProUGUI text;

    private void Start()
    {
        foreach(var button in allBlocks)
        {
            SOChooser script = button.GetComponent<SOChooser>();
            if(TowerParts.Instance != null)
            {
                if (!TowerParts.Instance.UnlockedTower(script.so.name))
                {
                    button.SetActive(false);
                }
                else
                {
                    script.amount = TowerParts.Instance.GetTowerAmount(script.so.name);
                    script.text.text = script.amount.ToString();
                }
            }
            
        }
    }

    public void ChooseGrid()
    {
        if (selectedBlock != null)
        {
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            SOExtractor soExtractor = selectedButton.GetComponentInChildren<SOExtractor>();
            if (soExtractor.blockSO == null)
            {
                soExtractor.blockSO = selectedBlock;
                soExtractor.originButton = originButton;
                soExtractor.wasExtracted = false;
                selectedButton.transform.localScale = currentMouseBlock.transform.localScale;
                selectedButton.transform.rotation = currentMouseBlock.transform.rotation;
                if (currentMouseBlock != null) Destroy(currentMouseBlock);
                selectedBlock = null;
            }
            else if(soExtractor.blockSO != selectedBlock)
            {
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
                soExtractor.blockSO = selectedBlock;
                selectedButton.transform.localScale = currentMouseBlock.transform.localScale;
                selectedButton.transform.rotation = currentMouseBlock.transform.rotation;
                if (currentMouseBlock != null) Destroy(currentMouseBlock);
                selectedBlock = null;
            }
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
            SOExtractor gridBlockSOE = gridBlocks[i].GetComponentInChildren<SOExtractor>();
            BlockScriptableObjects gridBlockSO = gridBlockSOE.blockSO;
            if (gridBlockSO != emptyBlock && gridBlockSO != null)
            {
                towerToSpawn[i] = gridBlockSO;
                originButtons[i] = gridBlockSOE.originButton;
                storedPartRotations[i] = gridBlocks[i].transform.rotation;
                storedPartScales[i] = gridBlocks[i].transform.localScale;
                notEmpty = true;
            }
            else
            {
                towerToSpawn[i] = null;
                originButtons[i] = null;
                storedPartRotations[i] = Quaternion.Euler(1, 1, 1);
                storedPartScales[i] = Vector3.zero;
            }
        }

        if(notEmpty)
        {
            GameObject currentTower = Instantiate(towerTemplate);
            TowerInitialiser tI = currentTower.GetComponent<TowerInitialiser>();
            tI.towerParts = towerToSpawn;
            tI.originalButtons = originButtons;
            tI.towerPartsRotations = storedPartRotations;
            tI.towerPartsScales = storedPartScales;
            tI.puzzlePieceAmount = puzzlePieceAmount;
            tI.timePerEarn = timePerEarn;
            tI.synergyText = synergyText;
            tI.layoutBonusText = layoutBonusText;
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
                tI.blockLayout.Add(gridBlocks[i].GetComponentInChildren<SOExtractor>().gridIndex);
            }
            buildUI.SetActive(false);
            Time.timeScale = 1;
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

    public void ToggleUI()
    {
        if (buildUI.activeSelf == false)
        {
            buildUI.SetActive(true);
            removeUI.SetActive(false);
            moveUI.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            buildUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ToggleMoveUI()
    {
        if (moveUI.activeSelf == false)
        {
            moveUI.SetActive(true);
            removeUI.SetActive(false);
            buildUI.SetActive(false);
            Time.timeScale = 0;
            isMoving = true;
        }
        else
        {
            moveUI.SetActive(false);
            Time.timeScale = 1;
            isMoving = false;
        }
    }

    public void ToggleRemoveUI()
    {
        if (removeUI.activeSelf == false)
        {
            removeUI.SetActive(true);
            moveUI.SetActive(false);
            buildUI.SetActive(false);
            Time.timeScale = 0;
            isRemoving = true;
        }
        else
        {
            removeUI.SetActive(false);
            Time.timeScale = 1;
            isRemoving = false;
        }
    }

    public void Toggle2xSpeed()
    {
        if(Time.timeScale == 1)
        {
            
            Time.timeScale = 2;
        }
        else if(Time.timeScale == 2)
        {
            Time.timeScale = 1;
        }
    }

    public void ToggleSynergyLookupUI()
    {
        if(lookUpSynergyUI.activeSelf)
        {
            lookUpSynergyUI.SetActive(false);
            buildUI.SetActive(true);
        }
        else
        {
            lookUpSynergyUI.SetActive(true);
            buildUI.SetActive(false);
        }
    }
}
