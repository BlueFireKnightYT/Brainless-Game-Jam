using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static BlockScriptableObjects;


public class TowerInitialiser : MonoBehaviour
{
    public BlockScriptableObjects[] towerParts;
    public GameObject[] originalButtons;
    public Quaternion[] towerPartsRotations;
    public Vector3[] towerPartsScales;
    GameObject[] spawnPoints;
    public GameObject partTemplate;
    BuilderUI buildUI;
    int totalBonusPieces;
    int totalCost;
    Vector2Int lastPosition;
    Vector2Int currentposition;

    [Header("Text Components")]
    public TextMeshProUGUI synergyText;
    public TextMeshProUGUI layoutBonusText;

    bool placed;
    public bool canPlace;
    public List<GameObject> lowestPart = new List<GameObject>();
    public LayerMask groundLayer;
    public LayerMask towerLayer;
    List<GameObject> children = new List<GameObject>();
    public bool isOverlapping;

    public int puzzlePieceAmount;
    public int timePerEarn;
    public List<Vector2Int> blockLayout;
    string presetName;

    bool blocksPath;


    bool MatchesPreset(List<Vector2Int> current, List<Vector2Int> preset)
    {
        return current.Count == preset.Count &&
               current.All(pos => preset.Contains(pos));
    }

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        buildUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BuilderUI>();
        foreach (BuilderUI.Preset preset in buildUI.presets)
        {
            if (MatchesPreset(blockLayout, preset.parts))
            {
                layoutBonusText.text = ("Activated: " + preset.presetName + " Layout Bonus!");
                presetName = preset.presetName;
            }
        }
        BuildTower();
    }
    public void BuildTower()
    {
        for (int i = 0; i < towerParts.Length; i++)
        {
            int x= 0;
            int y = 0;
            if (i <= 2)
            {
                x = i;
                y = 0;
            }
            else if (i <= 5)
            {
                x = i - 3;
                y = 1;
            }
            else
            {
                x = i - 6;
                y = 2;
            }
                

            Vector2 relativePos = new Vector2(x, y);
            if(towerParts[i] != null)
            {
                GameObject currentPart = Instantiate(partTemplate, relativePos, towerPartsRotations[i], transform);
                currentPart.GetComponent<PartSOUser>().partSO = towerParts[i];
                totalBonusPieces += currentPart.GetComponent<PartSOUser>().partSO.bonusPieces;
                totalCost += currentPart.GetComponent<PartSOUser>().partSO.cost;
                currentPart.GetComponent<PartSOUser>().originButton = originalButtons[i];
                StartCoroutine(ApplyBonus(presetName, currentPart.GetComponent<PartSOUser>()));
                if (towerPartsScales[i] == new Vector3(1, -1, 1))
                {
                    currentPart.GetComponent<SpriteRenderer>().flipY = true;
                }
                children.Add(currentPart);
            }
            if (towerParts[i] == null)
            {
                children.Add(null);
            }
        }

        foreach(GameObject currentPart in children)
        {
            if (currentPart == null) continue;

            Vector2Int pos = GetGridPos(currentPart);
            Vector2Int belowPos = pos + Vector2Int.down;

            bool hasBlockBelow = children.Any(c => c != null && GetGridPos(c) == belowPos);

            if (!hasBlockBelow)
            {
                lowestPart.Add(currentPart);
            }

            currentPart.GetComponent<PartSOUser>().bonusPieces = totalBonusPieces;

            if(currentPart.GetComponent<PartSOUser>().partSO.synergyList.Count != 0)
            {
                Synergy synergy = FindSynergy(children, currentPart.GetComponent<PartSOUser>().partSO);

                if (synergy != null)
                {
                    synergyText.text = ("Activated: " + synergy.SynergyName + " Synergy!");
                    StartCoroutine(ApplySynergy(synergy.SynergyName, currentPart.GetComponent<PartSOUser>()));
                }
            }
            
        }

    }

    private void Update()
    {
        
        if (!placed)
        {
            //Moves tower with mouse
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            int gridX = Mathf.RoundToInt(mouseWorldPos.x);
            int gridY = Mathf.RoundToInt(mouseWorldPos.y);

            Vector2 towerPos = new Vector2(gridX, gridY);
            transform.position = towerPos;

            //Gets base grid position of tower
            Vector2Int basePos = new Vector2Int(gridX, gridY);

            //Checks if any part is overlapping anything
            isOverlapping = !CanPlacePart(basePos);
            canPlace = !isOverlapping;

            if (canPlace && !blocksPath)
            {
                for (int i = 0; i < children.Count; i++)
                { 
                    if(children[i] == null) continue;
                    children[i].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            else 
            { 
                for (int i = 0; i < children.Count; i++) 
                {
                    if (children[i] == null) continue;
                    children[i].GetComponent<SpriteRenderer>().color = Color.red; 
                } 
            }

            lastPosition = basePos;
            if(currentposition != lastPosition)
            {
                currentposition = lastPosition;
                OnGridChange(basePos);
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && !placed && !blocksPath)
        {

            placed = true;
            //Occupies the slots of all parts
            for (int i = 0;i < children.Count; i++)
            {
                if (children[i] == null) continue;
                GridManager.Instance.Occupy(new Vector2Int(Mathf.RoundToInt(children[i].transform.position.x), Mathf.RoundToInt(children[i].transform.position.y)));
                children[i].GetComponent<PartSOUser>().OnPlace();
            }
            if (puzzlePieceAmount > 0)
            {
                StartCoroutine(PuzzlePieceEarner());
            }
            synergyText.text = null;
            layoutBonusText.text = null;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.transform.parent == this.gameObject.transform)
            {
                if (buildUI.isRemoving)
                {
                    if (PuzzlePieceManager.puzzlePieces >= 5)
                    {
                        for (int i = 0; i < children.Count; i++)
                        {
                            if (children[i] == null) continue;
                            children[i].GetComponent<PartSOUser>().originButton.GetComponent<SOChooser>().amount += 1;
                            GridManager.Instance.Unoccupy(new Vector2Int(Mathf.RoundToInt(children[i].transform.position.x), Mathf.RoundToInt(children[i].transform.position.y)));
                        }
                        PuzzlePieceManager.puzzlePieces -= 5;
                        Destroy(this.gameObject);
                    }
                }
                if (buildUI.isMoving)
                {
                    if(PuzzlePieceManager.puzzlePieces >= 5)
                    {
                        buildUI.ToggleMoveUI();
                        for (int i = 0; i < children.Count; i++)
                        {
                            if (children[i] == null) continue;
                            GridManager.Instance.Unoccupy(new Vector2Int(Mathf.RoundToInt(children[i].transform.position.x), Mathf.RoundToInt(children[i].transform.position.y)));
                        }
                        PuzzlePieceManager.puzzlePieces -= 5;
                        placed = false;
                    }
                }
                
            }
        }
    }
    bool CanPlacePart(Vector2Int basePos)
    {
        bool supportedBlock = false;
        //Occupancy check and Ground check on all parts
        for (int i = 0; i < towerParts.Length; i++)
        {
            if (towerParts[i] == null) continue;
            int x = 0;
            int y = 0;
            if (i <= 2)
            {
                x = i;
                y = 0;
            }
            else if (i <= 5)
            {
                x = i - 3;
                y = 1;
            }
            else
            {
                x = i - 6;
                y = 2;
            }

            Vector2Int pos = basePos + new Vector2Int(x, y);

            if (GridManager.Instance.IsOccupied(pos))
                return false;

            if (lowestPart.Contains(children[i]))
            {

                Vector2Int belowPos = pos + Vector2Int.down;

                if (GridManager.Instance.IsOccupied(belowPos))  
                {
                    supportedBlock = true;
                }
            }


        }
        if(supportedBlock) return true;
        return false;
    }

    public Vector2Int GetGridPos(GameObject obj)
    {
        return new Vector2Int(
        Mathf.RoundToInt(obj.transform.position.x),
        Mathf.RoundToInt(obj.transform.position.y)
    
        );
    }

    IEnumerator PuzzlePieceEarner()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePerEarn);
            PuzzlePieceManager.puzzlePieces += puzzlePieceAmount;
        }
    }

    IEnumerator ApplyBonus(string presetName, PartSOUser partSO)
    {
        yield return new WaitForSeconds(0.1f);
        
        if (presetName == "Castle")
        {
            partSO.hp *= 2;
        }
        if(presetName == "Wheel")
        {
            partSO.attackSpeed /= 1.5f;
        }
        if(presetName == "Spike")
        {
            partSO.damage = Mathf.RoundToInt(partSO.damage * 1.25f);
        }
        if(presetName == "Flower")
        {
            partSO.attackSpeed /= 1.25f;
        }
        if(presetName == "Corners")
        {
            partSO.duration *= 2f;
        }
        if(presetName == "Donut")
        {
            partSO.projectileAmount += 1;
        }
        if(presetName == "Tower")
        {
            partSO.piercing += 1;
        }
        if(presetName == "Table")
        {
            partSO.bonusPieces += 5;
        }
        if(presetName == "Chair")
        {
            partSO.bonusPieces += 3;
        }
        if(presetName == "Hat")
        {
            partSO.piercing += 1;
        }
        if(presetName == "Stairs")
        {
            partSO.attackSpeed *= 1.5f;
        }
    }

    Synergy FindSynergy(List<GameObject> objects, BlockScriptableObjects blockSO)
    {
        return blockSO.synergyList.FirstOrDefault(s =>
            s.synergyBlockID.All(id =>
                objects.Any(obj =>
                    obj != null &&
                    obj.GetComponent<PartSOUser>() != null &&
                    obj.GetComponent<PartSOUser>().partSO.objID == id
                )
            )
        );
    }

    IEnumerator ApplySynergy(string synergyName, PartSOUser partSO)
    {
        yield return new WaitForSeconds(0.1f);

        if(synergyName == "Bad Idea")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            partScript.explodeRadius *= 2;
            partScript.damage *= 2;
            partScript.duration /= 2;
        }

        if(synergyName == "Frostburn")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            if (partScript.activatesDOT)
            {
                partScript.activatesSlowness = true;
                partScript.slowModifier = .75f;
                partScript.slowTime = 3;
            }
            if (partScript.activatesSlowness)
            {
                partScript.activatesDOT = true;
                partScript.damagePerRepeat = 3;
                partScript.repeats = 3;
                partScript.damageSpeed = 1;
            }
        }
        if(synergyName == "Sky Piercer")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            partScript.piercing = 4;
            partScript.bulletSpeed *= 2;
            partScript.damage *= 2;
        }
        if(synergyName == "Brainless Storm")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            partScript.friendlyFire = true;
            partScript.piercing = 1;
            partScript.reverseSpeed = 1;
            partScript.reverseTime = 5;
        }
        if(synergyName == "Minigun")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            partScript.projectileAmount = 4;
            partScript.attackSpeed = 2;
        }
        if(synergyName == "Snowstorm")
        {
            PartSOUser partScript = partSO.GetComponent<PartSOUser>();
            partScript.shootsProjectiles = true;
        }
    }

    void OnGridChange(Vector2Int basePos)
    {
        HashSet<Vector2Int> towerPartsLocations = new HashSet<Vector2Int>();
        for (int i = 0; i < towerParts.Length; i++)
        {
            if (towerParts[i] == null) continue;
            int x = 0;
            int y = 0;
            if (i <= 2)
            {
                x = i;
                y = 0;
            }
            else if (i <= 5)
            {
                x = i - 3;
                y = 1;
            }
            else
            {
                x = i - 6;
                y = 2;
            }

            Vector2Int pos = basePos + new Vector2Int(x, y);
            towerPartsLocations.Add(pos);
        }
        int blockedPaths = 0;
        foreach (GameObject spawnpoint in spawnPoints)
        {
            CreateEnemyPath cEP = spawnpoint.GetComponent<CreateEnemyPath>();
            List<Vector2Int> path = cEP.ePF.FindPath(WorldToGrid(spawnpoint.transform.position), WorldToGrid(cEP.player.transform.position), towerPartsLocations);
            if(path == null || path.Count == 0)
            {
                blockedPaths++;
            }
        }
        if (blockedPaths != 0) blocksPath = true;
        if (blockedPaths == 0) blocksPath = false;
    }
    Vector2Int WorldToGrid(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y)
        );
    }
}
