using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    public int health;
    public Sprite halfHeartSprite;
    public Image[] hearts;

    public void TakeDamage(int damage)
    {
        health = -damage;
        if(health == 1 || health == 3 || health == 5)
        {
            hearts[Mathf.RoundToInt(health/2)].sprite = halfHeartSprite;
        }
        else
        {
            hearts[health/2 + 1].sprite = null;
            hearts[health/2 + 2].sprite = null;
        }
    }
}
