using System;
using System.Collections;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool isLeft;
    public bool isMoving;
    public int health;
    public int puzzlePieceReward;
    public int damage;
    public int speed;
    Rigidbody2D rb;
    Collider2D coll;
    public string towerTag;
    public string playerTag;
    bool canDamage;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        isMoving = true;
        canDamage = true;

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

        if(health <= 0)
        {
            PuzzlePieceManager.puzzlePieces += puzzlePieceReward;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(towerTag))
        {
            isMoving = false;
            rb.linearVelocityX = 0;
            StartCoroutine(DamageTowerPart(damage, collision.GetComponent<PartSOUser>()));
        }
        if (collision.gameObject.CompareTag(playerTag) && canDamage)
        {
            canDamage = false;
            coll.enabled = false;
            isMoving = false;
            rb.linearVelocityX = 0;
            collision.GetComponent<HealthHandler>().ChangeHealth(1);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DamageTowerPart(int damage, PartSOUser partScript)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            partScript.TakeDamage(damage);
            if (partScript.dead)
            {
                isMoving = true;
                StopCoroutine(DamageTowerPart(damage, partScript));
            }
        } 
    }
}
