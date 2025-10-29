using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, Items
{
    // Variables for coin collection and point value
    public static event System.Action<int> GemCollect;
    public int points = 5;
    public void Collect() // Collection of coins that give points for progress, plays the collection sounds, and destroys the coin
    {
        GemCollect.Invoke(points);
        SoundFXManager.Play("Coins");
        Destroy(gameObject);
    }
}
