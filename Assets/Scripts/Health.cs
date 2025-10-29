using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Variables for hearts (healthy and injured/dead)
    public Image heart;
    public Sprite healthyHeart;
    public Sprite deadHeart;

    private List<Image> hearts = new List<Image>();

    public void SetFullHealth(int fullHealth) // Creates hearts and sets to full health 
    {
        foreach(Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < fullHealth; i++) 
        {
            Image newHeart = Instantiate(heart, transform);
            newHeart.sprite = healthyHeart;
            newHeart.color = Color.red;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currHealth) // Updates hearts on UI to white when hit
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currHealth)
            {
                hearts[i].sprite = healthyHeart;
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].sprite = deadHeart;
                hearts[i].color = Color.white;
            }
        }
    }
}
