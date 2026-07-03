using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using static Unity.VisualScripting.Metadata;

public class TowerInitialiser : MonoBehaviour
{
    public BlockScriptableObjects[] towerParts;
    public GameObject partTemplate;

    bool placed;
    public bool canPlace;
    public GameObject lowestPart;
    public LayerMask groundLayer;
    public LayerMask groundAndTowerLayer;
    SpriteRenderer[] childrenSr;
    public bool isOverlapping;


    public void BuildTower()
    {
        int height = 3;
        for (int i = 0; i < towerParts.Length; i++)
        {
            int x = i % 3;
            int y = i / 3;

            Vector2 relativePos = new Vector2(x, y);
            if(towerParts[i] != null)
            {
                GameObject currentPart = Instantiate(partTemplate, relativePos, Quaternion.identity, transform);
                currentPart.GetComponent<PartSOUser>().partSO = towerParts[i];
                if(y < height)
                {
                    height = y;
                    lowestPart = currentPart;
                }
            }
        }
    }

    private void Update()
    {
        childrenSr = gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (!placed)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            int gridX = Mathf.RoundToInt(mouseWorldPos.x);
            int gridY = Mathf.RoundToInt(mouseWorldPos.y);

            Vector2 towerPos = new Vector2(gridX, gridY);
            transform.position = towerPos;

            isOverlapping = false;
            for (int i = 0; i < childrenSr.Length; i++)
            { 
                Collider2D collHit = Physics2D.OverlapBox(childrenSr[i].gameObject.transform.position, new Vector2(.9f, .9f), 0, groundLayer);
                if (collHit != null && !collHit.transform.IsChildOf(transform)) 
                { 
                    isOverlapping = true; break;
                }
            }

            RaycastHit2D hit = Physics2D.BoxCast(lowestPart.transform.position, Vector2.one * .5f, 0, Vector2.down, 1f, groundLayer);
            if(hit.collider != null)
            {
                canPlace = true;
            }
            else
            {
                canPlace = false;
            }

            if (canPlace && !isOverlapping)
            {
                for (int i = 0; i < childrenSr.Length; i++) { if (childrenSr[i] != this.GetComponent<SpriteRenderer>()) childrenSr[i].color = Color.white; }
            }
            else { for (int i = 0; i < childrenSr.Length; i++) { if (childrenSr[i] != this.GetComponent<SpriteRenderer>()) childrenSr[i].color = Color.red; } }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && !isOverlapping)
        {
                placed = true;
        }    
    }
}
