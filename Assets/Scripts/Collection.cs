using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Collection : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) // Collect items when colliding with them
    {
        Items item = collision.GetComponent<Items>();
        if (item != null)
        {
            item.Collect();
        }
    }
}
