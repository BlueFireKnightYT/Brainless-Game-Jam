using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public int damage;
    public bool explodes;
    public int size;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour behaviourScript = collision.gameObject.GetComponent<EnemyBehaviour>();
            behaviourScript.health -= damage;
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
