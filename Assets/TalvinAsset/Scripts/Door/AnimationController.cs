using UnityEngine;

namespace Door
{
    public class AnimationController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private MyDoorController myDoorController;

        #endregion

        public void ChangeDoor()
        {
            myDoorController.doorOpen = !myDoorController.doorOpen;
        }
    }
}
