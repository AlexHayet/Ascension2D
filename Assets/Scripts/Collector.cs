using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Collector : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) // Collect items when colliding with them
    {
        if (!IsOwner) return; // This checks for the proper client so both players won't send the same rpc

        Items item = collision.GetComponent<Items>();
        if (item != null)
        {
            //item.Collect();
            //CollectItemServerRpc(item.NetworkObjectId);

            // A network object must exist here because collection is being done across the network, rather than the old singleplayer logic
            // This allows for both players to collect coins
            NetworkObject netObj = collision.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                CollectItemServerRpc(netObj.NetworkObjectId);
            }
        }
    }

    // Rpc used for authoritative actions across the server
    [ServerRpc]
    void CollectItemServerRpc(ulong itemId)
    {
        // The NetworkObject Id is necessary in order to keep track of the exact gem being collected
        NetworkObject itemObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[itemId];

        // Despawns the gem being collected for all players
        itemObj.Despawn();
    }
}
