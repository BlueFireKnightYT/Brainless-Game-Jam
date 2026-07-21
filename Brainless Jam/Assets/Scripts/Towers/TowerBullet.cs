using System.Collections;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public int damage;
    public int size;
    public float duration;
    public int amountPierced;
    public int piercing;
    GameObject lastHitEnemy;
    public GameObject parentBlock;
    bool hit = false;
    public int bonusPieces;

    public bool activatesExplosion;
    public float explodeRadius;
    public int explodePierce;

    public bool activatesSlow;
    public float slowModifier;
    public int slowTime;

    public bool activatesDOT;
    public int burnDamage;
    public int burnRepeats;
    public float burnSpeed;

    public bool activatesReverse;
    public float reverseTime;
    public float reverseSpeed;
    public bool friendlyFire;

    public bool shootsProjectiles;
    public GameObject projectile;

    public bool canHitCamo;
    public bool isFire;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy") && lastHitEnemy != collision.gameObject)
        {
            EnemyBehaviour behaviourScript = collision.gameObject.GetComponent<EnemyBehaviour>();
            if((!behaviourScript.isCamouflaged && !behaviourScript.immuneToFire) || (behaviourScript.isCamouflaged && canHitCamo) || (behaviourScript.immuneToFire && !isFire))
            {
                behaviourScript.health -= damage;
                if ((behaviourScript.puzzlePieceReward - behaviourScript.basePieces) < bonusPieces)
                {
                    behaviourScript.puzzlePieceReward = (behaviourScript.basePieces + bonusPieces);
                }
                lastHitEnemy = collision.gameObject;
                amountPierced++;
                if (amountPierced >= piercing)
                {
                    hit = true;
                    Destroy(this.gameObject);
                }
                if (activatesExplosion) ExplodeBomb();

                if (activatesSlow)
                {
                    if (!behaviourScript.slowed)
                    {
                        behaviourScript.ApplySlow(slowModifier, slowTime);
                    }
                }

                if (activatesDOT)
                {
                    if (!behaviourScript.DOTActive)
                    {
                        behaviourScript.StartDOT(burnDamage, burnSpeed, burnRepeats);
                    }
                }
                if (activatesReverse)
                {
                    EnemyPathMover pathScript = collision.GetComponent<EnemyPathMover>();
                    pathScript.ActivateReverse(reverseTime, reverseSpeed, friendlyFire);
                }
            }
        }
        if (collision.gameObject.CompareTag("Tower") && collision.gameObject != parentBlock)
        {
            hit = true;
            if(activatesExplosion) ExplodeBomb();
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        if (shootsProjectiles) StartCoroutine(ShootProjectile(projectile));
        if(activatesExplosion) Invoke("ExplodeBomb", duration);
        Destroy(this.gameObject, duration);
    }

    IEnumerator ShootProjectile(GameObject projectile)
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.transform.up = transform.up;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.up * 10;
        }
    }

    void ExplodeBomb()
    {
        int enemiesExploded = 0;
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.down, 0f);
        {
            foreach (RaycastHit2D explodeHit in hit)
            {
                if (explodeHit.collider.CompareTag("Enemy") && enemiesExploded <= explodePierce)
                {
                    enemiesExploded++;
                    EnemyBehaviour behaviourScript = explodeHit.collider.GetComponent<EnemyBehaviour>();
                    if ((behaviourScript.puzzlePieceReward - behaviourScript.basePieces) < bonusPieces)
                    {
                        behaviourScript.puzzlePieceReward = (behaviourScript.basePieces + bonusPieces);
                    }
                    behaviourScript.health -= Mathf.RoundToInt(damage * .5f);
                }
            }
        }

    }
}
