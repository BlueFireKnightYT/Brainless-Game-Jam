using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    public int health;
    public int thorns;
    public GameObject shieldBlock;
    PartSOUser shieldScript;

    private void Start()
    {
        shieldScript = shieldBlock.GetComponent<PartSOUser>();
    }
    public void TakeDamage(int damage, EnemyBehaviour behaviour)
    {
        health -= damage;
        if(health <= 0)
        {
            shieldScript.StartCoroutine(shieldScript.SpawnObject(shieldScript.partSO.obj, shieldScript.partSO.replaceTime));
            Destroy(this.gameObject);
        }
        behaviour.health -= thorns;
    }
}
