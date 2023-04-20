using Inventory;
using Item;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace User
{
    public class BatState : NetworkBehaviour
    {
        #region Variables

        [HideInInspector] public InventoryItem inventoryItem;

        private InventoryManager _inventory;
        private UserInputs _userInputs;

        [SerializeField] private GameObject batToSet;
        [SerializeField] private ItemData bat;
        private ItemData _currentItem;
        
        private GameObject _currentBat;
        public NetworkVariable<bool> hasBatInHand = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        #endregion

        private void Awake()
        {
            ChangeStateBatServerRpc(false);
            _inventory = GetComponentInChildren<InventoryManager>();
            _userInputs = GetComponentInChildren<UserInputs>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_userInputs.use))
            {
                if (!hasBatInHand.Value)
                {
                    HasNotBatInHand();
                }
                else
                {
                    HasBatInHand();
                }
            }
        }

        private void HasNotBatInHand()
        {
            _currentItem = inventoryItem.itemData;
            if (_currentItem != bat) return;
                
            hasBatInHand.Value = true;
            _inventory.canChangeSelectedSlot = false;
            ChangeStateBatServerRpc(true);
        }

        public void HasBatInHand()
        {
            ChangeStateBatServerRpc(false);
            _inventory.canChangeSelectedSlot = true;
            hasBatInHand.Value = false;
        }

        public void DestroyBat()
        {
            ChangeStateBatServerRpc(false);
            _inventory.canChangeSelectedSlot = true;
            hasBatInHand.Value = false;

            inventoryItem.slot.itemData = null;
            Destroy(inventoryItem.gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateBatServerRpc(bool state)
        {
            AuxClientRpc(state);
        }
        
        [ClientRpc]
        private void AuxClientRpc(bool state)
        {
            batToSet.SetActive(state);
        }
    }
}
