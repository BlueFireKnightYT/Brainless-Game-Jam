using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockScriptableObjects", menuName = "Scriptable Objects/BlockScriptableObjects")]
public class BlockScriptableObjects : ScriptableObject
{
    [Header("Main")]
    public string nameObject;
    public int hp;
    public Sprite sprite;
    public int cost;

    [Header("Attack")]
    public bool doesAttack;
    public int damage;
    public float attackSpeed;
    public float bulletSpeed;
    public float bulletDuration;
    public GameObject bulletPrefab;

    [Header("Explosion")]
    public float explosionRadius;

    [Header("Puzzle Pieces")]
    public int piecesPerTime;
    public int timePerEarn;

    [System.Serializable]
    public class Synergy
    {
        public string SynergyName;
        public int synergyBlockID;
    }

    [Header("Synergy")]
    public int objID;
    public List<Synergy> synergyList;

}
