using System;
using UnityEngine;
using User;

namespace Inventory
{
    public class InventoryState : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject mainInventoryGroup;
        [SerializeField] private GameObject button;
        [SerializeField] private UserInputs playerInputs;

        [NonSerialized] public bool IsOpen;

        #endregion
    
        private void Start()
        {
            mainInventoryGroup.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(playerInputs.inventory))
            {
                ChangeInventoryState(!IsOpen);
            }
        }

        public void ChangeInventoryState(bool state)
        {
            mainInventoryGroup.SetActive(state);
            button.SetActive(!state);
            IsOpen = state;
        }
    }
}
