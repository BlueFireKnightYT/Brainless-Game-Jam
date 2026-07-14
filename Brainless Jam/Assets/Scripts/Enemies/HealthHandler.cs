using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    public int health;
    public Sprite halfHeartSprite;
    public Sprite fullHeartSprite;
    public Image[] hearts;

    public void ChangeHealth(int change)
    {
        health -= change;
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        UpdateUIHearts();
    }

    void UpdateUIHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            int value = health - (i * 2);

            if (value >= 2)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else if(value == 1)
            {
                hearts[i].sprite = halfHeartSprite;
            }
            else
            {
                Image heart = hearts[i];

                Color c = heart.color;
                c.a = 0f;
                heart.color = c;
            }
        }
    }
}
