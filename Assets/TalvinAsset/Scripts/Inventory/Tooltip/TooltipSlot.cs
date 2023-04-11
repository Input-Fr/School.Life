using Item;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.Tooltip
{
    public class TooltipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [SerializeField] public ItemData itemData;
        [SerializeField] private Tooltip tooltip;

        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemData != null)
            {
                tooltip.Show(itemData.description, itemData.name);   
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Hide();
        }

        public Tooltip GetTooltip()
        {
            return tooltip;
        }
    }
}
