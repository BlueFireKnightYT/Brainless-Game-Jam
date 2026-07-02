using UnityEngine;

public class CraftingTowers : MonoBehaviour
{
    public BlockScriptableObjects[,] towerSO = new BlockScriptableObjects[3, 3];

    public GameObject emptyTowerPrefab;
    public GameObject BasePiece;

    GameObject baseTower;

    int arrayNum1 = 0;
    int arrayNum2 = 0;

    void Start()
    {
        baseTower = Instantiate(emptyTowerPrefab, transform.position, transform.rotation);
    }

    void OnPieceAdd()
    {
        GameObject addedPiece = Instantiate(BasePiece, baseTower.transform);
        addedPiece.transform.localPosition = new Vector2(-1 + arrayNum1, -1 + arrayNum2);

        SOExtractor soExtractor = addedPiece.GetComponent<SOExtractor>();
        soExtractor.blockSO = towerSO[arrayNum1, arrayNum2];
    }
}
