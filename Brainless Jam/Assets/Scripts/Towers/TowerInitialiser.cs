using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using static BlockScriptableObjects;


public class TowerInitialiser : MonoBehaviour
{
    public BlockScriptableObjects[] towerParts;
    public Quaternion[] towerPartsRotations;
    public Vector3[] towerPartsScales;
    public GameObject partTemplate;
    BuilderUI buildUI;

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


    bool MatchesPreset(List<Vector2Int> current, List<Vector2Int> preset)
    {
        return current.Count == preset.Count &&
               current.All(pos => preset.Contains(pos));
    }

    private void Start()
    {
        buildUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BuilderUI>();
        foreach (BuilderUI.Preset preset in buildUI.presets)
        {
            if (MatchesPreset(blockLayout, preset.parts))
            {
                Debug.Log("Activated: " + preset.presetName);
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

            if(currentPart.GetComponent<PartSOUser>().partSO.synergyList.Count != 0)
            {
                Synergy synergy = FindSynergy(children, currentPart.GetComponent<PartSOUser>().partSO);

                if (synergy != null)
                {
                    Debug.Log("Activated: " + synergy.SynergyName);
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

            if (canPlace)
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
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && !placed)
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

    Vector2Int GetGridPos(GameObject obj)
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
    }

    Synergy FindSynergy(List<GameObject> objects, BlockScriptableObjects blockSO)
    {
        return blockSO.synergyList.FirstOrDefault(s =>
        objects.Any(obj => obj != null && obj.GetComponent<PartSOUser>() != null && obj.GetComponent<PartSOUser>().partSO.objID == s.synergyBlockID));
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
    }
}
