using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public int damage;
    public int size;
    public GameObject parentBlock;
    bool hit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour behaviourScript = collision.gameObject.GetComponent<EnemyBehaviour>();
            behaviourScript.health -= damage;
            hit = true;
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Tower") && collision.gameObject != parentBlock)
        {
            hit = true;
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
