using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public float holdTime = 1f; // Time to hold to load level
    public Image circle;

    private float holdTimer = 0;
    private bool holding = false;

    public static event Action HoldCompletion;

    private void Update()
    {
        if (holding) // Fill the circle over the 1s duration when e is being held
        {
            holdTimer += Time.deltaTime;
            circle.fillAmount = holdTimer / holdTime;

            if (holdTimer >= holdTime) // Load the next level
            {
                HoldCompletion.Invoke();
                CancelHold();
            }
        }
    }

    public void Hold(InputAction.CallbackContext context) // Allows for holding of e to start next level and cancelling the hold
    {
        if (context.started) // Holding is true when button is held
        {
            Debug.Log("Hold Started");
            holding = true;
        }

        else if (context.canceled) // Empty hold Timer when let go
        {
            Debug.Log("Hold Cancelled");
            CancelHold();
        }
    }

    private void CancelHold() // When letting go of e, the next level call is cancelled
    {
        holding = false;
        holdTimer = 0;
        circle.fillAmount = 0;
    }
}
