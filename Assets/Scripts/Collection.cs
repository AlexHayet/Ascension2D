using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
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
