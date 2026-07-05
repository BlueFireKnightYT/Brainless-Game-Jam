using System.Collections;
using UnityEngine;

public class PartSOUser : MonoBehaviour
{
    public BlockScriptableObjects partSO;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = partSO.sprite;
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
    }
}
