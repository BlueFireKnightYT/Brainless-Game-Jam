using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

public class PartSOUser : MonoBehaviour
{
    public BlockScriptableObjects partSO;
    TowerInitialiser tI;
    SpriteRenderer sr;
    Collider2D coll;
    public bool dead;
    public int hp;
    public float attackSpeed;
    public int bonusPieces;
    Vector3 rightOfBlock;

    [Header("Projectile Properties")]
    public int damage;
    public float duration;
    public float projectileAmount;
    public int piercing;
    public float bulletSpeed;
    public bool shootsProjectiles;
    [Header("Explosion Properties")]
    public bool activatesExplosion;
    public float explodeRadius;
    public int explodePierce;
    [Header("Damage over Time Properties")]
    public bool activatesDOT;
    public int damagePerRepeat;
    public int repeats;
    public float damageSpeed;
    [Header("Slow Properties")]
    public bool activatesSlowness;
    public float slowModifier;
    public int slowTime;
    [Header("Reverse Properties")]
    public bool activatesReverse;
    public float reverseSpeed;
    public float reverseTime;
    public bool friendlyFire;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = partSO.sprite;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        tI = transform.parent.GetComponent<TowerInitialiser>();
        hp = partSO.hp;
        attackSpeed = partSO.attackSpeed;
        damage = partSO.damage;
        duration = partSO.bulletDuration;
        piercing = partSO.piercing;
        bulletSpeed = partSO.bulletSpeed;

        activatesExplosion = partSO.activatesExplosion;
        explodeRadius = partSO.explosionRadius;
        explodePierce = partSO.explosionPierce;

        activatesDOT = partSO.activatesDOT;
        damagePerRepeat = partSO.damagePerRepeat;
        repeats = partSO.repeats;
        damageSpeed = partSO.damageSpeed;

        projectileAmount = partSO.bulletAmount;

        activatesSlowness = partSO.activatesSlowness;
        slowModifier = partSO.slowModifier;
        slowTime = partSO.slowTime; 

        activatesReverse = partSO.activatesReverse;
        reverseSpeed = partSO.reverseSpeed;
        reverseTime = partSO.reverseTime;
        friendlyFire = partSO.enableFriendlyFire;
    }

    IEnumerator ShootBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);

            for (int i = 0; i < projectileAmount; i++)
            {
                FireBullet();

                if (i < projectileAmount-1)
                    yield return new WaitForSeconds(0.1f);
            }
        }

    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(partSO.bulletPrefab, transform.position + transform.right * 0.5f, Quaternion.Euler(0, 0, 0));
        if (bullet.GetComponent<TowerBullet>() != null)
        {
            TowerBullet script = bullet.GetComponent<TowerBullet>();
            script.damage = damage;
            script.parentBlock = this.gameObject;
            script.duration = duration;
            script.piercing = piercing;
            script.bonusPieces = bonusPieces;
            if (activatesExplosion)
            {
                script.activatesExplosion = true;
                script.explodeRadius = explodeRadius;
                script.explodePierce = explodePierce;
            }
            if (activatesDOT)
            {
                script.activatesDOT = true;
                script.burnDamage = damagePerRepeat;
                script.burnRepeats = repeats;
                script.burnSpeed = damageSpeed;
            }
            if (activatesSlowness)
            {
                script.activatesSlow = true;
                script.slowModifier = slowModifier;
                script.slowTime = slowTime;
            }
            if (activatesReverse)
            {
                script.activatesReverse = true;
                script.reverseSpeed = reverseSpeed;
                script.reverseTime = reverseTime;
                script.friendlyFire = friendlyFire;
            }
            if (shootsProjectiles)
            {
                script.shootsProjectiles = true;
            }
        }
        bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * 10 * bulletSpeed;
        bullet.transform.up = transform.right;
        if(this.gameObject.GetComponent<SpriteRenderer>().flipY == true)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void OnPlace()
    {
        if (partSO.doesAttack)
        {
            StartCoroutine(ShootBullet());
        }
        coll.enabled = true;
        rightOfBlock = transform.right + transform.position;
        if (partSO.spawnsObject)
        {
            StartCoroutine(SpawnObject(partSO.obj, partSO.replaceTime));
        }
    }
    public IEnumerator SpawnObject(GameObject obj, float replaceTime)
    {
        yield return new WaitForSeconds(replaceTime);
        GameObject shield = Instantiate(obj, rightOfBlock, Quaternion.identity);
        shield.GetComponent<ShieldHandler>().shieldBlock = this.gameObject;
    }
}
