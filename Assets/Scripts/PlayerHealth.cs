using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Variables related to Player health and visuals of getting hit
    public int fullHealth = 3;
    private int currHealth;
    public Health healthUI;
    private SpriteRenderer spriteRenderer;
    public static event Action Death;

    void Start() // On start, set health to full
    {
        currHealth = fullHealth;
        healthUI.SetFullHealth(fullHealth);

        spriteRenderer = GetComponent<SpriteRenderer>();
        Controller.GameRestart += ResetHP;
    }

    private void OnTriggerEnter2D(Collider2D collision) // Take damage when colliding with an enemy and play the TakeDamage sound
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDmg(enemy.damage);
            SoundFXManager.Play("TakeDamage");
        }
    }

    private void TakeDmg(int damage) // Subtracts helath when taking damage, Player dies when reaching 0 health
    {
        currHealth -= damage;
        healthUI.UpdateHearts(currHealth);

        // Flash red when taking damage
        StartCoroutine(DmgVisual());

        if (currHealth <= 0)
        {
            Death.Invoke();
        }
    }

    private IEnumerator DmgVisual() // Player flashes red when taking damage from enemy
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }

    void ResetHP() // Resets HP on level restart
    {
        currHealth = fullHealth;
        healthUI.SetFullHealth(fullHealth);
    }
}
