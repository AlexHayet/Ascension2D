using UnityEngine;
using Unity.Netcode;

public class Collector : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;

        Gem gem = collision.GetComponent<Gem>();

        if (gem != null)
        {
            NetworkObject netObj = gem.GetComponent<NetworkObject>();

            if (netObj != null)
            {
                CollectGemServerRpc(netObj.NetworkObjectId);
            }
        }
    }

    [ServerRpc]
    void CollectGemServerRpc(ulong gemId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(gemId, out NetworkObject obj))
        {
            Gem gem = obj.GetComponent<Gem>();

            if (gem != null)
            {
                gem.Collect(OwnerClientId);
            }
        }
    }
}