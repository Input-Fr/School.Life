using UnityEngine;

namespace Door
{
    public class MyDoorController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Animator doorAnimator;
        [SerializeField] public bool doorOpen = false;

        #endregion

        public void PlayAnimation()
        {
            if (!doorOpen)
            {
                doorAnimator.Play("DoorOpen", 0, 0.0f);
                doorOpen = true;
            }

            else
            {
                doorAnimator.Play("DoorClose", 0, 0.0f);
                doorOpen = false;
            }
        }

        public bool AnimatorIsPlaying()
        {
            return doorAnimator.GetCurrentAnimatorStateInfo(0).length >
                   doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
