using UnityEngine;
using User;
using Door;
using System;
using TMPro;
using UnityEngine.UI;
using Canvas;
using Inventory;
using System.Threading.Tasks;

namespace Item
{
    public class ItemsInteraction : Detection 
    {
        #region Variables

        private float viewAngle;
        private LayerMask environment;        
        GameObject boxDeMesRoubignoles;
        DoorsInteraction _door;
        GameObject Player;

        #endregion
    
        public ItemsInteraction()
        {
            maxDistanceInteraction = 2.2f;
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

        protected override async void Awake()
        {
            var layer1 = 3;
            var layer2 = 7;
            var layermask1 = 1 << layer1;
            var layermask2 = 1 << layer2;
            environment = layermask1 | layermask2;
            viewAngle = 30;
            maxDistanceInteraction = 2.2f;
            mask = 6;
            var Players = GameObject.FindGameObjectsWithTag("subPlayer");
            Player = Players[Players.Length-1];
            boxDeMesRoubignoles = FindInActiveObjectByTag("txtInteraction");
            inventory = Player.GetComponent<InventoryManager>();
            userInputs = Player.GetComponent<UserInputs>();
            text = boxDeMesRoubignoles.GetComponent<TextInteraction>();
            Camera = GetComponent<Camera>();
            await Task.Delay(50);
            _door = GetComponent<Door.DoorsInteraction>();
            currentTransform = this.transform;
        }

        protected override void Update()
        {
            if (_door == null || currentTransform == null) return; 
            InitialiseVariables();
            
            if (DetectTarget())
            {
                ShowInformation();
                UserInteraction();
            }
            else if (!SameObjects)
            {
                HideInformation();
            }
            else if (text.IsActive() && !_door.isDetected)
            {
                text.Hide();
            }
        }

        protected override void InitialiseVariables()
        {
            Origin = currentTransform.position;
            Direction = currentTransform.forward;
            PreviousObject = CurrentObject;
        }

        protected override bool DetectTarget()
        {
            GameObject currentObject = null;
            // ReSharper disable once Unity.PreferNonAllocApi
            RaycastHit[] allHits = Physics.SphereCastAll(Origin, maxDistanceInteraction, Direction, maxDistanceInteraction, mask);

            if (allHits.Length > 0)
            {
                float minAngle = 180;
                Plane[] cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera);
                foreach (RaycastHit hit in allHits)
                {
                    Bounds hitBounds = hit.collider.bounds;
                    if (!GeometryUtility.TestPlanesAABB(cameraFrustum, hitBounds)) continue;

                    Vector3 directionToTarget = (hit.transform.position - Origin).normalized;

                    float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                    if (Physics.Raycast(Origin, directionToTarget, distanceToTarget, environment)) continue;
                
                    float angle = Vector3.Angle(transform.forward, directionToTarget);
                    if (!(angle < viewAngle / 2)) continue;
                
                    if (minAngle < 180)
                    {
                        if (!(angle < minAngle)) continue;
                
                        currentObject = hit.transform.gameObject;
                        minAngle = angle;
                    }
                    else
                    {
                        currentObject = hit.transform.gameObject;
                        minAngle = angle;
                    }
                }
            }

            
            if (currentObject == CurrentObject)
            {
                if (PreviousObject != CurrentObject) PreviousObject = CurrentObject;
                else if (!CurrentObject) PreviousObject = null;
            }
            CurrentObject = currentObject;
            SameObjects = CurrentObject == PreviousObject;
            isDetected = (bool)CurrentObject;
            return isDetected;
        }
    
        protected override void ShowInformation()
        {
            if (SameObjects) return;

            if ((bool)PreviousObject)
            {
                PreviousObject.GetComponent<Outline>().enabled = false;
                text.Hide();
            }

            CurrentObject.GetComponent<Outline>().enabled = true;
        
            string header = GetItem().itemData.name;
            string content = $"PICK UP  [{userInputs.interaction}]";
            text.Show(Color.white, CurrentObject.transform.position + new Vector3(0, 0.5f, 0), content, header);
            
        }

        protected override void UserInteraction()
        {
            if (Input.GetKeyDown(userInputs.interaction))
            {
                GetItem().Pickup(inventory);
                HideInformation();
                CurrentObject = null;
            }
        }

        protected override void HideInformation()
        {
            if ((bool)PreviousObject)
            {
                PreviousObject.GetComponent<Outline>().enabled = false;
            }
            text.Hide();
            
        }

        private Item GetItem()
        {
            return CurrentObject.GetComponent<Item>();
        }
    }
}
