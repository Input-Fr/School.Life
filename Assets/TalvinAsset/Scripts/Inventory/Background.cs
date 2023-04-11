using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class Background : MonoBehaviour, IDropHandler
    {
        #region Variables

        [SerializeField] private Transform parent;
        [SerializeField] private Transform dropPoint;

        #endregion
    
    
        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            GameObject prefab = item.slot.itemData.prefab;
        
            for (int i = 0; i < item.numberItem; i++)
            {
                GameObject instantiatedItem = Instantiate(prefab, parent);
                instantiatedItem.transform.position = dropPoint.position;
                instantiatedItem.name = item.slot.itemData.name;
            }
        
            item.isDropped = true;
            item.slot.itemData = null;
        }
    }
}
