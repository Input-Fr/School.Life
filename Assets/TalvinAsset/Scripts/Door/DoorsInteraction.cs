using Item;
using UnityEngine;
using User;

namespace Door
{
    public class DoorsInteraction : Detection
    {
        #region Variables

        private MyDoorController _door;
        private ItemsInteraction _items;
    
        private const string UnlockedDoor = "Unlocked";
        private const string Key = "Key";

        private ItemData _previousItemData;
        private ItemData _currentItemData;

        private bool _sameItemData;
        private bool _isLocked;

        #endregion

        public DoorsInteraction()
        {
            maxDistanceInteraction = 4f;
            PreviousObject = null;
            CurrentObject = null;
            isDetected = false;
        }

        protected override void Awake()
        {
            Camera = GetComponent<Camera>();
            _items = GetComponent<ItemsInteraction>();
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
            _isLocked = isDetected && !CurrentObject.CompareTag(UnlockedDoor);
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
                if ((bool)_currentItemData && _currentItemData.name == Key) 
                    text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
                else 
                    text.Show(Color.red, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
            
            }
        
            else
            {
                if (!(bool)_currentItemData) return;

                if (_currentItemData.name == Key)
                {
                    HideInformation();
                    text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
                    return;
                }
                
                HideInformation();
                text.Show(Color.red, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), Content());
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
            if (_currentItemData.name != Key) return;
            
            inventory.UseSelectedItem();
            CurrentObject.tag = UnlockedDoor;
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
