using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class Background : MonoBehaviour, IDropHandler
    {
        #region Variables

        [SerializeField] private PlayerNetwork player;
        [SerializeField] private Transform dropPoint;

        #endregion
    
    
        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        
            for (int i = 0; i < item.numberItem; i++)
            {
                player.InstantiateItem(item.slot.itemData.prefabInScene,dropPoint.position);
            }
        
            item.isDropped = true;
            item.slot.itemData = null;
        }
    }
}
