using System.Collections.Generic;
using User;
using Item;
using UnityEngine;
using UnityEngine.Serialization;
using Canvas;
using Inventory;
using System.Threading.Tasks;
using System;

namespace Door
{
    public class DoorsInteraction : Detection
    {
        #region Variables

        private MyDoorController _door;
        private ItemsInteraction _items;

        private ItemData _previousItemData;
        private ItemData _currentItemData;

        private bool _sameItemData;
        private bool _isLocked;

        private GameObject _doorObj;
        GameObject Player;
        GameObject boxDeMesRoubignoles;


        #endregion

        public DoorsInteraction()
        {
            maxDistanceInteraction = 4f;
            PreviousObject = null;
            CurrentObject = null;
            isDetected = false;
        }

        GameObject FindInActiveObjectByTag(string tag)
        {
            GameObject res = null;
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].CompareTag(tag))
                    {
                        res = objs[i].gameObject;
                    }
                }
            }
            return res;
        }

        protected override void Awake()
        {
            var Players = GameObject.FindGameObjectsWithTag("subPlayer");
            Player = Players[Players.Length-1];
            boxDeMesRoubignoles = FindInActiveObjectByTag("txtInteraction");
            inventory = Player.GetComponent<InventoryManager>();
            userInputs = Player.GetComponent<UserInputs>();
            text = boxDeMesRoubignoles.GetComponent<TextInteraction>();
            Camera = GetComponent<Camera>();
            _items = GetComponent<ItemsInteraction>();
            currentTransform = this.transform;
        }

        protected override void Update()
        {
            InitialiseVariables();

            if (_items.isDetected)
            {
                DoorNotDetected();
            }
            else if (DetectTarget())
            {
                _door = GetDoor();
                ShowInformation();
                UserInteraction();
            }
            else if (!SameObjects)
            {
                HideInformation();
            }
        }

        protected override void InitialiseVariables()
        {
            Origin = currentTransform.position;
            Direction = currentTransform.TransformDirection(Vector3.forward);
            PreviousObject = CurrentObject;
        }

        protected override bool DetectTarget()
        {
            GameObject currentObject = null;
            if (Physics.Raycast(Origin, Direction, out RaycastHit hit, maxDistanceInteraction, mask))
            {
                currentObject = hit.collider.gameObject;
            }

            CurrentObject = currentObject;
            isDetected = (bool)CurrentObject;
            _isLocked = isDetected && !CurrentObject.CompareTag("Unlocked");
            SameObjects = CurrentObject == PreviousObject;
            return isDetected;
        }

        private void DoorNotDetected()
        {
            CurrentObject = null;
            isDetected = false;
            _isLocked = false;
            SameObjects = CurrentObject == PreviousObject;
        }
    
        protected override void ShowInformation()
        {
            if (_isLocked) ShowLockedDoorInfo();
            else ShowUnlockedDoorInfo();
        }

        private void ShowLockedDoorInfo()
        {
            _previousItemData = _currentItemData;
            _currentItemData = inventory.GetSelectedItem();
            _sameItemData = (bool)_currentItemData && _currentItemData == _previousItemData;

            if (_sameItemData && text.IsActive()) return;

            if (!text.IsActive())
            {
                if ((bool)_currentItemData && _currentItemData.itemName == "Key") 
                    text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
                else 
                    text.Show(Color.red, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
            }
        
            else
            {
                if ((bool)_currentItemData && _currentItemData.itemName == "Key")
                {
                    HideInformation();
                    text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
                }
                else
                {
                    HideInformation();
                    text.Show(Color.red, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
                }
            }
        }

        private void ShowUnlockedDoorInfo()
        {
            if (_door.AnimatorIsPlaying() || text.IsActive()) return;
        
            text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
        }

        protected override void UserInteraction()
        {
            if (!Input.GetKeyDown(userInputs.interaction)) return;
        
            if (_isLocked) UserLockedDoorInteract();
            else UserUnlockedDoorInteract();
        }

        private void UserLockedDoorInteract()
        {
            if (!(bool)_currentItemData) return;
            if (_currentItemData.itemName != "Key") return;
            
            inventory.UseSelectedItem();
            _doorObj = CurrentObject;
            _door.ChangeVariableServerRpc(false);
            HideInformation();
        }

        private void UserUnlockedDoorInteract()
        {
            if (_door.AnimatorIsPlaying()) return;
        
            _door.PlayAnimation();
            HideInformation();
        }

        protected override void HideInformation()
        {
            text.Hide();
        }
    
        private string Content()
        {
            string state;
        
            if (_isLocked) state = "UNLOCK";
            else state = _door.doorOpen ? $"CLOSE" : "OPEN";
        
            return $"{state}  [{userInputs.interaction}]";
        }

        private MyDoorController GetDoor()
        {
            return CurrentObject.GetComponent<MyDoorController>();
        }
    }
}
