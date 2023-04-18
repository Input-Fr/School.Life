using Inventory;
using Unity.Netcode;
using UnityEngine;

namespace Item
{
    public class Item : NetworkBehaviour
    {
        #region Variables
        
        public ItemData itemData;

        #endregion

        public bool Pickup(InventoryManager inventory)
        {
            if (inventory.AddItem(itemData))
            {
                DestroyItemServerRpc();
                return true;
            }
            
            Debug.Log("Can't ADD the Item");
            return false;
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyItemServerRpc()
        {
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
