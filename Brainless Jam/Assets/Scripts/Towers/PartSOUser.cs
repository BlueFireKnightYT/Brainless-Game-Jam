using System.Collections;
using UnityEngine;

public class PartSOUser : MonoBehaviour
{
    public BlockScriptableObjects partSO;
    SpriteRenderer sr;
    Collider2D coll;
    public bool dead;
    public int hp;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = partSO.sprite;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        hp = partSO.hp;
    }

    IEnumerator ShootBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(partSO.attackSpeed);
            GameObject bullet = Instantiate(partSO.bulletPrefab, transform.position + transform.right *0.5f, Quaternion.Euler(0, 0, 0));
            bullet.GetComponent<TowerBullet>().damage = partSO.damage;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * 10 * partSO.bulletSpeed;
        }
    }

    public void OnPlace()
    {
        if (partSO.doesAttack)
        {
            StartCoroutine(ShootBullet());
            print("Coroutine Started");
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
