using UnityEngine;
using UnityEngine.UI;
namespace Canvas
{
    public class TextInteraction : MonoBehaviour
    {
        #region Variables
    
        [SerializeField] private Text headerField;
        [SerializeField] private Text descriptionField;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int maxCharacter = 80;

        public bool isLocalPlayer;
        private Camera cam;
        private Vector3 _position;
    
        #endregion

        private void Awake()
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (IsActive())
            {
                UpdatePosition();
            }
        }

        public void Show(Color color, Vector3 position, string content, string header = "")
        {
            _position = position;
            if (header == "")
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.text = header;
                headerField.color = color;
                headerField.gameObject.SetActive(true);
            }

            descriptionField.text = content;
            descriptionField.color = color;
        
            int headerLength = headerField.text.Length;
            int contentLength = descriptionField.text.Length;

            layoutElement.enabled = headerLength > maxCharacter || contentLength > maxCharacter;

            UpdatePosition();
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdatePosition()
        {
            transform.position = cam.WorldToScreenPoint(_position);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}
