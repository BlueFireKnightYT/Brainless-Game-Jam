using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool isLeft;
    public bool isMoving;
    Rigidbody2D rb;
    Collider2D coll;
    public string towerTag;
    public string playerTag;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        isMoving = true;
    }
    void Update()
    {
        if (isMoving)
        {
            if (isLeft)
            {
                rb.linearVelocityX = 1;
            }
            else
            {
                rb.linearVelocityX = -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(towerTag))
        {
            isMoving = false;
            rb.linearVelocityX = 0;
        }
        if (collision.gameObject.CompareTag(playerTag))
        {
            isMoving = false;
            rb.linearVelocityX = 0;
            collision.GetComponent<HealthHandler>().TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
