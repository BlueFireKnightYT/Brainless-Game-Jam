using UnityEngine;

public class TowerBomb : MonoBehaviour
{
    public int damage;
    public float duration;
    public float explodeRadius;
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
            ExplodeBomb();
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Tower") && collision.gameObject != parentBlock)
        {
            hit = true;
            ExplodeBomb();
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Invoke(nameof(ExplodeBomb), duration);
        Destroy(this.gameObject, duration);
    }

    void ExplodeBomb()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.down, 0f);
        {
            foreach(RaycastHit2D explodeHit in hit)
            {
                if (explodeHit.collider.CompareTag("Enemy"))
                {
                    explodeHit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= Mathf.RoundToInt(damage / .75f);
                    print("kaboom");
                }
            }
        }
        
    }
}
