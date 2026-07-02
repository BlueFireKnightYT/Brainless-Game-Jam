using UnityEngine;

public class CraftingTowers : MonoBehaviour
{
    public GameObject[,] towerArray = new GameObject[3, 3];

    public GameObject emptyTowerPrefab;
    public GameObject BasePiece;

    public BlockScriptableObjects baseBlockSO;

    BlockScriptableObjects selectedSO;
    GameObject baseTower;

    int arrayNum1 = 0;
    int arrayNum2 = 0;

    void Start()
    {
        baseTower = Instantiate(emptyTowerPrefab, transform.position, transform.rotation);

        selectedSO = baseBlockSO;
        towerArray[arrayNum1, arrayNum2] = BasePiece;
        OnPieceAdd();
        towerArray[1, 0] = BasePiece;
        OnPieceAdd();
    }


    void Update()
    {

    }

    void OnPieceAdd()
    {
        GameObject addedPiece = Instantiate(towerArray[arrayNum1, arrayNum2], baseTower.transform);
        addedPiece.transform.localPosition = new Vector2(-1 + arrayNum1, -1 + arrayNum2);

        SOExtractor soExtractor = addedPiece.GetComponent<SOExtractor>();
        soExtractor.blockSO = selectedSO;
    }
}
