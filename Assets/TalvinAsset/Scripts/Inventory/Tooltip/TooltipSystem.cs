using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject tooltip;
        [SerializeField] private Text headerField;
        [SerializeField] private Text descriptionField;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private int maxCharacter = 80;

        public bool canShow = true;

        #endregion

        private void Start()
        {
            tooltip.SetActive(false);
            SetTooltipPosition();
        }

        private void Update()
        {
            SetTooltipPosition();
        }

        public void Show(string content, string header = "")
        {
            if (!canShow) return;
        
            SetText(content, header);
            tooltip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }

        private void SetText(string content, string header = "")
        {
            if (header == "")
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.text = header;
                headerField.gameObject.SetActive(true);
            }

            descriptionField.text = content;
        
            int headerLength = headerField.text.Length;
            int contentLength = descriptionField.text.Length;

            layoutElement.enabled = headerLength > maxCharacter || contentLength > maxCharacter;
        }

        private void SetTooltipPosition()
        {
            Vector2 mousePosition = Input.mousePosition;

            float pivotX = mousePosition.x / Screen.width;
            float pivotY = mousePosition.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = mousePosition;
        }
    }
}
