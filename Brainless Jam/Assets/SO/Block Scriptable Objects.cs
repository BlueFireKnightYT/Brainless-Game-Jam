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

    [Header("Object Spawning")]
    public bool spawnsObject;
    public GameObject obj;
    public float replaceTime;

    [Header("Attack")]
    public bool doesAttack;
    public int damage;
    public float attackSpeed;
    public float bulletSpeed;
    public float bulletDuration;
    public GameObject bulletPrefab;
    public int bulletAmount;
    public int piercing;

    [Header("Explosion")]
    public bool activatesExplosion;
    public float explosionRadius;
    public int explosionPierce;

    [Header("Damage Over Time")]
    public bool activatesDOT;
    public int damagePerRepeat;
    public int repeats;
    public float damageSpeed;

    [Header("Slowness")]
    public bool activatesSlowness;
    public float slowModifier;
    public int slowTime;

    [Header("Reverse")]
    public bool activatesReverse;
    public float reverseSpeed;
    public float reverseTime;
    public bool enableFriendlyFire;

    [Header("Puzzle Pieces")]
    public int bonusPieces;

    [System.Serializable]
    public class Synergy
    {
        public string SynergyName;
        public int[] synergyBlockID;
    }

    [Header("Synergy")]
    public int objID;
    public List<Synergy> synergyList;

}
