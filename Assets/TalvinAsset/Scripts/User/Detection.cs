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
    
        protected InventoryManager inventory;
        protected LayerMask mask;
        protected UserInputs userInputs;
        protected TextInteraction text;

        protected float maxDistanceInteraction;
        protected Transform currentTransform;
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
