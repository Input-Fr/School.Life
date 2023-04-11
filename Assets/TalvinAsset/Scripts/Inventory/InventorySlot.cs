using Inventory.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        #region Variables

        public Image image;
        public Color selectedColor, notSelectedColor;

        #endregion
    

        private void Awake()
        {
            Deselect();
        }
    
        public void Select()
        {
            image.color = selectedColor;
        }
    
        public void Deselect()
        {
            image.color = notSelectedColor;
        }
    
        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem firstItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        
            if (transform.childCount == 0)
            {
                firstItem.parentAfterDrag = transform;
            }
            else
            {
                firstItem.itemToExchange = GetComponentInChildren<InventoryItem>();
                firstItem.itemToExchange.parentAfterDrag = firstItem.parentAfterDrag;
            
                firstItem.parentAfterDrag = transform;
            }

            TooltipSlot firstSlot = firstItem.slot;
            TooltipSlot secondSlot = GetComponent<TooltipSlot>();

            (firstSlot.itemData, secondSlot.itemData) = (secondSlot.itemData, firstSlot.itemData);
        }
    }
}
