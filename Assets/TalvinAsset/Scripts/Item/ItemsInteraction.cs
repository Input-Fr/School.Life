using UnityEngine;
using User;

namespace Item
{
    public class ItemsInteraction : Detection
    {
        #region Variables

        [SerializeField] private float viewAngle;
        [SerializeField] private LayerMask environment;

        #endregion
    
        public ItemsInteraction()
        {
            maxDistanceInteraction = 2.2f;
            PreviousObject = null;
            CurrentObject = null;
            isDetected = false;
        }

        protected override void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        protected override void Update()
        {
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

            CurrentObject = currentObject;
            isDetected = (bool)CurrentObject;
            SameObjects = CurrentObject == PreviousObject;
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
