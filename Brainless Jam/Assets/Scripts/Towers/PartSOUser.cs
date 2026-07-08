using System.Collections;
using UnityEngine;

public class PartSOUser : MonoBehaviour
{
    public BlockScriptableObjects partSO;
    SpriteRenderer sr;
    Collider2D coll;
    public bool dead;
    public int hp;
    public float attackSpeed;

    [Header("Projectile Properties")]
    public int damage;
    public float duration;
    public float explodeRadius;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = partSO.sprite;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        hp = partSO.hp;
        attackSpeed = partSO.attackSpeed;
        damage = partSO.damage;
        duration = partSO.bulletDuration;
        explodeRadius = partSO.explosionRadius;
    }

    IEnumerator ShootBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            GameObject bullet = Instantiate(partSO.bulletPrefab, transform.position + transform.right *0.5f, Quaternion.Euler(0, 0, 0));
            if(bullet.GetComponent<TowerBullet>() != null)
            {
                bullet.GetComponent<TowerBullet>().damage = damage;
                bullet.GetComponent<TowerBullet>().parentBlock = this.gameObject;
            }
            if(bullet.GetComponent<TowerBomb>() != null)
            {
                bullet.GetComponent<TowerBomb>().damage = damage;
                bullet.GetComponent<TowerBomb>().parentBlock = this.gameObject;
                bullet.GetComponent<TowerBomb>().duration = duration;
                bullet.GetComponent<TowerBomb>().explodeRadius = explodeRadius;
            }
            bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * 10 * partSO.bulletSpeed;
        }
    }

    public void OnPlace()
    {
        if (partSO.doesAttack)
        {
            StartCoroutine(ShootBullet());
        }
        coll.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            dead = true;
            Destroy(this.gameObject);
        }
    }
}
