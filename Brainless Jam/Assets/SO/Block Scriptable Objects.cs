using UnityEngine;

[CreateAssetMenu(fileName = "BlockScriptableObjects", menuName = "Scriptable Objects/BlockScriptableObjects")]
public class BlockScriptableObjects : ScriptableObject
{
    [Header("Main")]
    public string nameObject;
    public int hp;
    public Sprite sprite;

    [Header("Attack")]
    public bool doesAttack;
    public int damage;
    public float attackSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    [Header("Puzzle Pieces")]
    public int piecesPerTime;
    public int timePerEarn;
}
