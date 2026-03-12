using UnityEngine;
using Unity.Netcode;

public class Gem : NetworkBehaviour
{
    // Score for gem collection
    public int value = 1;

    public void Collect(ulong playerId)
    {
        // Server handles collection
        if (!IsServer) return;

        // Add score to the player
        GameManager.Instance.AddScore(playerId, value);

        // Find spawner to make a new gem
        GemSpawner spawner = FindObjectOfType<GemSpawner>();

        if (spawner != null)
        {
            // Respawn a new gem after 2 seconds
            spawner.RespawnGem(gameObject, 2f);
        }
        else
        {
            // Despawn gems if there is no spawner
            NetworkObject.Despawn();
        }
    }
}