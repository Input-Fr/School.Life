using Inventory;
using UnityEngine;
using Unity.Netcode;
namespace Item
{
    public class Item : NetworkBehaviour
    {
        #region Variables

        public ItemData itemData;

        #endregion

        


        public void Pickup(InventoryManager inventory)
        {
            if (inventory.AddItem(itemData))
            {
                DestroyObjServerRpc();
                
            }
            else
            {
                Debug.Log("Can't ADD the Item, Inventory full");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void DestroyObjServerRpc()
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
        public void EnableOutline()
        {
            GetComponent<Outline>().enabled = true;
        }

        public void DisableOutline()
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
