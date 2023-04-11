using Canvas;
using Inventory;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Unity.Netcode;
namespace User
{
    public abstract class Detection : NetworkBehaviour
    {
        #region Variables

        protected Camera Camera;
    
        [SerializeField] protected InventoryManager inventory;
        [SerializeField] protected LayerMask mask;
        [SerializeField] protected UserInputs userInputs;
        [SerializeField] protected TextInteraction text;

        [SerializeField] protected float maxDistanceInteraction;
        [SerializeField] protected Transform currentTransform;
        protected Vector3 Origin;
        protected Vector3 Direction;

        protected GameObject PreviousObject;
        protected GameObject CurrentObject;

        protected bool SameObjects = true;
        [HideInInspector] public bool isDetected;
    
        #endregion
        protected abstract void Awake();

        protected abstract void Update();

        protected abstract void InitialiseVariables();

        protected abstract bool DetectTarget();
    
        protected abstract void ShowInformation();

        protected abstract void UserInteraction();

        protected abstract void HideInformation();


    }
}
