using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true;

    // Movement
    public float moveSpeed = 5f;
    float horizontalMovement;
    float verticalMovement;

    // Jump
    public float jumpStrength = 10f;
    public int jumpCount = 2;
    private int remainingJumps;

    // Grounded
    public Transform groundPosition;
    public Vector2 groundRange = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool grounded;

    // Gravity
    public float defGravity = 2;
    public float highSpeed = 18f;
    public float speedMult = 2f;

    // Wall Check
    public Transform wallPosition;
    public Vector2 wallRange = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    // Wall Slide/Jump
    public float slideSpeed = 2;
    bool sliding;
    bool wallJumping;
    float wallJumpDir;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpStrength = new Vector2(5f, 10f);

    //private void Start() // Sets rigidbody of player on start
    //{
    //rb = GetComponent<Rigidbody2D>();
    //}

    public override void OnNetworkSpawn() // Sets rigidbody of player on spawn
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() // Continuously updates the statuses below, calculates player velocity
    {
        if (!IsOwner) return;

        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        Grounded();
        Gravity();
        WallSlide();
        WallJump();

        if (!wallJumping) // Changes velocity when changing directions
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            ChangeDirection();
        }
    }

    public void Move(InputAction.CallbackContext context) // Gives the player horizontal movement
    {
        //horizontalMovement = context.ReadValue<Vector2>().x;
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalMovement = -3.0f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalMovement = 3.0f;
        }
        else
        {
            horizontalMovement = 0;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            verticalMovement = -3.0f;
        }
        else if (Keyboard.current.upArrowKey.isPressed)
        {
            verticalMovement = 3.0f;
        }
        else
        {
            verticalMovement = 0;
        }
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(horizontalMovement, verticalMovement);
    }

    public void Jump(InputAction.CallbackContext context) // Allows the player to jump, hold jump, and wall jump
    {
        if (remainingJumps > 0) // If the player still has jumps
        {
            if (context.performed) // Hold for stronger jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);
                remainingJumps--;
            }
            else if (context.canceled) // Tap for smaller jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                remainingJumps--;
            }
        }

        // Wall Jumping
        if (context.performed && wallJumpTimer > 0f)
        {
            wallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDir * wallJumpStrength.x, wallJumpStrength.y); // Will now jump away from the wall when wal jumping
            wallJumpTimer = 0;

            // Forcing a change in direction on wall jump
            if (transform.localScale.x != wallJumpDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(WallJumpCancel), wallJumpTime + 0.1f); // Allows for jump again after wall jump for fluidity
        }

    }

    private void Grounded() // Check for the player being on the ground. Reset jumps if on the ground
    {
        if (Physics2D.OverlapBox(groundPosition.position, groundRange, 0, groundLayer))
        {
            remainingJumps = jumpCount;
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private bool WallCheck() // A check for walls to make sure the player is against one
    {
        return Physics2D.OverlapBox(wallPosition.position, wallRange, 0, wallLayer);
    }

    private void Gravity() // Sets gravity for player and how it effects them
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = defGravity = speedMult; // Fall faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -highSpeed));
        }
        else
        {
            rb.gravityScale = defGravity;
        }
    }

    private void ChangeDirection() // Allows the player model to change directions while moving left and right
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void WallSlide() // When pressing against a wall the player can slide down it at a slower rate than falling
    {
        if (!grounded & WallCheck() & horizontalMovement != 0)
        {
            sliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -slideSpeed)); // Sets limit to fall speed
        }
        else
        {
            sliding = false;
        }
    }

    private void WallJump() // Allows for wall jumping when against a wall
    {
        if (sliding)
        {
            wallJumping = false;
            wallJumpDir = transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(WallJumpCancel));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void WallJumpCancel() // Can cancel the wall jump
    {
        wallJumping = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundPosition.position, groundRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallPosition.position, wallRange);
    }
}