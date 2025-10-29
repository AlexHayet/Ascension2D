using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Variables related to enemy stats and target
    public Transform player;
    public float speed = 2f;
    public float jumpStrength = 2f;
    public LayerMask groundLayer;

    public Rigidbody2D rb;
    private bool grounded;
    private bool jumpCheck;

    public int damage = 1;

    void Start() // Sets rigidbody of enemy on start
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() // Updates various conditions for enemy
    {
        // Condition for enemy being grounded
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        // Direction of movement
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Keep moving horizontally
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // Look diagonally down to detect upcoming gap
        Vector2 downForward = new Vector2(direction, -0.5f).normalized;
        RaycastHit2D groundAhead = Physics2D.Raycast(transform.position, downForward, 1.5f, groundLayer);

        // Detect player above
        bool playerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        // If at an edge, or the player is above, jump
        if (!groundAhead.collider || playerAbove)
        {
            jumpCheck = true;
        }
    }

    private void FixedUpdate() // Allows for more movement on the enemy
    {
        // If grounded and jumpchec are true
        if (jumpCheck && grounded)
        {
            jumpCheck = false;
            Vector2 direction = (player.position - transform.position).normalized; // Track player
            Vector2 force = new Vector2(direction.x * jumpStrength, jumpStrength); // Force of the enemy's jump
            rb.AddForce(force, ForceMode2D.Impulse); // Force of the enemy's jump
        }
    }
}
