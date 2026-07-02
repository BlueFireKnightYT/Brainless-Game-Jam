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
    public float damage;
    public float attackSpeed;
    public GameObject bulletPrefab;
}
