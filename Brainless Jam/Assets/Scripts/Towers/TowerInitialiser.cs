using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class TowerInitialiser : MonoBehaviour
{
    public BlockScriptableObjects[] towerParts;
    public Quaternion[] towerPartsRotations;
    public Vector3[] towerPartsScales;
    public GameObject partTemplate;

    bool placed;
    public bool canPlace;
    public List<GameObject> lowestPart = new List<GameObject>();
    public LayerMask groundLayer;
    public LayerMask towerLayer;
    List<GameObject> children = new List<GameObject>();
    public bool isOverlapping;

    public int puzzlePieceAmount;
    public int timePerEarn;
        


    private void Start()
    {
        
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
            print(PuzzlePieceManager.puzzlePieces);
        }
    }

}
