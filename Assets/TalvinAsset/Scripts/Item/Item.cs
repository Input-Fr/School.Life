using Inventory;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        #region Variables

        public ItemData itemData;

        #endregion

        public void Pickup(InventoryManager inventory)
        {
            if (inventory.AddItem(itemData))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Can't ADD the Item, Inventory full");
            }
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
