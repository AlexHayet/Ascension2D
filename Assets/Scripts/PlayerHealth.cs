using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    // Variables related to Player health and visuals of getting hit
    public int fullHealth = 3;
    //private int currHealth;
    public NetworkVariable<int> currHealth = new NetworkVariable<int>(); // Currhealth becomes a network variable in multiplayer in order for the health to track across players
    public Health healthUI;
    private SpriteRenderer spriteRenderer;
    public static event Action Death;

    void Start() // On start, set health to full
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (IsServer)
        {
            currHealth.Value = fullHealth;
        }

        currHealth.OnValueChanged += OnHealthChanged;

        //currHealth = fullHealth;
        healthUI.SetFullHealth(fullHealth);

        currHealth.OnValueChanged += OnHealthChanged;

        //spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!IsServer) return;

        currHealth.Value -= damage;
        healthUI.UpdateHearts(currHealth.Value);
        //currHealth -= damage;
        //healthUI.UpdateHearts(currHealth);

        // Flash red when taking damage
        StartCoroutine(DmgVisual());

        if (currHealth.Value <= 0) // Player death when health is <= 0
        {
            Death?.Invoke();
        }
        //if (currHealth <= 0)
        //{
            //Death.Invoke();
        //}
    }

    private IEnumerator DmgVisual() // Player flashes red when taking damage from enemy
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }

    void ResetHP() // Resets HP on level restart
    {
        if (!IsServer) return;

        currHealth.Value = fullHealth;
        healthUI.SetFullHealth(fullHealth);
    }

    void OnHealthChanged(int oldHealth, int newHealth) // Update HealthUI for MP
    {
        healthUI.UpdateHearts(newHealth);
    }
}
