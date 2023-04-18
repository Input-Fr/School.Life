using Inventory;
using Item;
using UnityEngine;
using User;

namespace Professors
{
    public class AttractProfessor : MonoBehaviour
    {
        #region Variables

        [SerializeField] private ItemData phone;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private InventoryManager inventory;
        [SerializeField] private UserInputs userInputs;

        private CountDown _countDown;
        private InventorySlot _slot;
        private ItemData _currentItem;
        
        #endregion

        private void Update()
        {
            _currentItem = inventory.GetSelectedItem();
            if (_currentItem != phone || !Input.GetKeyDown(userInputs.use)) return;
            
            Debug.Log("ici");
            InventorySlot slot = inventory.GetSlot();
            if (_slot != slot || !_countDown)
            {
                _slot = slot;
                _countDown = slot.GetComponentInChildren<CountDown>();
            }

            if (!_countDown.isFinsh) return;
            
            audioSource.Play();
            _countDown.Being(30);
            Sound sound = new Sound(transform.position, audioSource.maxDistance);
            Sounds.MakeSound(audioSource, sound);
        }
    }
}
