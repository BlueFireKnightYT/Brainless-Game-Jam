using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool isLeft;
    public bool isMoving;
    public int health;
    public int puzzlePieceReward;
    public int basePieces;
    public int damage;
    public float speed;
    Rigidbody2D rb;
    Collider2D coll;
    public string towerTag;
    public string playerTag;
    bool canDamage;
    public bool DOTActive;
    public bool slowed;
    public bool friendlyFire;
    public bool breakBlocks;
    public bool isCamouflaged;
    public bool immuneToFire;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        isMoving = true;
        canDamage = true;
        basePieces = puzzlePieceReward;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if ((player.transform.position.x - transform.position.x) < 0)
        {
            isLeft = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
            isLeft = true;
    }
    void Update()
    {

        if (health <= 0)
        {
            PuzzlePieceManager.puzzlePieces += puzzlePieceReward;
            Destroy(this.gameObject);
        }
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && canDamage)
        {
            canDamage = false;
            coll.enabled = false;
            isMoving = false;
            rb.linearVelocityX = 0;
            collision.GetComponent<HealthHandler>().ChangeHealth(1);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyBehaviour>().friendlyFire)
            {
                EnemyBehaviour behaviour = collision.gameObject.GetComponent<EnemyBehaviour>();
                speed = 0;
                StartCoroutine(DamageEnemy(damage, behaviour));
            }
            else if (friendlyFire)
            {
                EnemyBehaviour behaviour = collision.gameObject.GetComponent<EnemyBehaviour>();
                speed = 0;
                StartCoroutine(DamageEnemy(damage, behaviour));
            }
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            speed = 0;
            StartCoroutine(DamageShield(5, this, collision.gameObject.GetComponent<ShieldHandler>()));
        }
    }
    IEnumerator DamageShield(int damage, EnemyBehaviour behaviour, ShieldHandler shield)
    {
        while (shield != null)
        {
            yield return new WaitForSeconds(1);

            if (shield == null)
                break;

            shield.TakeDamage(damage, behaviour);
        }

        speed = 1;
    }
    IEnumerator DamageEnemy(int damage, EnemyBehaviour behaviour)
    {
        while ((true && behaviour.health >= 0 && behaviour != null) && (friendlyFire || behaviour.friendlyFire))
        {
            yield return new WaitForSeconds(1);
            behaviour.health -= damage;
        }
        StopCoroutine(DamageEnemy(damage, behaviour));
        speed = 1;
    }
    public void StartDOT(int damage, float time, int repeats)
    {
        StartCoroutine(TakeDamageOverTime(damage, time, repeats));
    }
    IEnumerator TakeDamageOverTime(int damage, float time, int repeats)
    {
        int count = 0;
        DOTActive = true;
        while (true && repeats != count)
        {
            yield return new WaitForSeconds(time);
            print("burn");
            health -= damage;
            count++;
        }
        StopCoroutine(TakeDamageOverTime(damage, time, repeats));
        print("stop");
    }

    public void ApplySlow(float speedModifier, int time)
    {
        StartCoroutine(SlowEnemy(speedModifier, time));
    }
    IEnumerator SlowEnemy(float speedModifier, int time)
    {
        slowed = true;
        speed *= speedModifier;
        yield return new WaitForSeconds(time);
        speed /= speedModifier;
        slowed = false;
    }
}
