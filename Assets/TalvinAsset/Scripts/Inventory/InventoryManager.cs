using Inventory.Tooltip;
using Item;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        #region Variables

        public InventorySlot[] inventorySlots;
    
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private Transform toolBar;
        [SerializeField] private Transform mainInventory;

        private int _selectedSlot = -1;
        [HideInInspector] public bool canChangeSelectedSlot = true;
        [HideInInspector] public bool hasSubject;

        #endregion

        private void Start()
        {
            inventorySlots = new InventorySlot[toolBar.childCount + mainInventory.childCount];
            for (int i = 0; i < toolBar.childCount; i++)
            {
                inventorySlots[i] = toolBar.GetChild(i).GetComponent<InventorySlot>();
            }

            for (int i = 0; i < mainInventory.childCount; i++)
            {
                inventorySlots[i + 4] = mainInventory.GetChild(i).GetComponent<InventorySlot>();
            }
        
            ChangeSelectedSlot(0);
        }

        private void Update()
        {
            if (Input.inputString != null)
            {
                bool isNumber = int.TryParse(Input.inputString, out int number);
                if (isNumber && number is > 0 and < 5)
                {
                    ChangeSelectedSlot(number - 1);
                }
            }
        }

        private void ChangeSelectedSlot(int newValue)
        {
            if (_selectedSlot >= 0)
            {
                inventorySlots[_selectedSlot].Deselect();
            }
        
            inventorySlots[newValue].Select();
            _selectedSlot = newValue;
        }
    
        public bool AddItem(ItemData itemData)
        {
            if (itemData.stackable)
            {
                foreach (InventorySlot slot in inventorySlots)
                {
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot == null || itemInSlot.itemData != itemData || itemInSlot.numberItem >= itemInSlot.itemData.nbItemByStack) continue;
                
                    itemInSlot.numberItem++;
                    itemInSlot.RefreshCount();
                    return true;
                }
            }

            if (itemData.multiple)
            {
                foreach (InventorySlot inventorySlot in inventorySlots)
                {
                    TooltipSlot slot = inventorySlot.gameObject.GetComponent<TooltipSlot>();
                    InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null) continue;

                    SpawnNewItem(itemData, inventorySlot);
                    slot.itemData = itemData;
                    if (itemData.itemName == "Subject")
                    {
                        hasSubject = true;
                    }
                    return true;
                }
            }
            else
            {
                foreach (InventorySlot inventorySlot in inventorySlots)
                {
                    TooltipSlot slot = inventorySlot.gameObject.GetComponent<TooltipSlot>();
                    if (slot.itemData == itemData) return false;
                    
                    InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null) continue;

                    SpawnNewItem(itemData, inventorySlot);
                    slot.itemData = itemData;
                    if (itemData.itemName == "Subject")
                    {
                        hasSubject = true;
                    }
                    return true;
                }
            }

            return false;
        }

        void SpawnNewItem(ItemData itemData, InventorySlot slot)
        {
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(itemData);
        }

        public ItemData GetSelectedItem()
        {
            InventorySlot slot = inventorySlots[_selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                ItemData itemData = itemInSlot.itemData;
                return itemData;
            }

            return null;
        }

        public void UseSelectedItem()
        {
            InventorySlot inventorySlot = inventorySlots[_selectedSlot];
            TooltipSlot slot = inventorySlot.GetComponent<TooltipSlot>();
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
        
            if (itemInSlot != null)
            {
                itemInSlot.numberItem--;
                if (itemInSlot.numberItem <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                    slot.itemData = null;
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
        }

        public InventorySlot GetSlot()
        {
            return inventorySlots[_selectedSlot];
        }
    }
}
