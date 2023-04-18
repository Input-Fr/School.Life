using System;
using Inventory.Tooltip;
using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using User;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variables

        [SerializeField] private Image image;
        [SerializeField] private Text countText;
        [SerializeField] private Tooltip.Tooltip tooltip;

        [HideInInspector] public ItemData itemData;
        [HideInInspector] public InventoryItem itemToExchange;
        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public TooltipSlot slot;
    
        [HideInInspector] public int numberItem = 1;
        [HideInInspector] public bool isDropped;
        
        #endregion

        private void Start()
        {
            tooltip = GetComponentInParent<TooltipSlot>().GetTooltip();
        }

        public void InitialiseItem(ItemData newItemData)
        {
            itemData = newItemData;
            image.sprite = itemData.image;
            RefreshCount();
        }

        public void RefreshCount()
        {
            countText.text = numberItem.ToString();
            bool textActive = numberItem > 1;
            countText.gameObject.SetActive(textActive);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            tooltip.canShow = false;
            tooltip.Hide();
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            slot = GetComponentInParent<TooltipSlot>();
            transform.SetParent(transform.parent.parent.parent.parent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDropped)
            {
                Destroy(gameObject);
                isDropped = false;
            }
            else
            {
                transform.SetParent(parentAfterDrag);

                if (itemToExchange != null)
                {
                    itemToExchange.transform.SetParent(itemToExchange.parentAfterDrag);
                    itemToExchange = null;
                }
            }
        
            image.raycastTarget = true;
            tooltip.canShow = true;
        }
    }
}
