using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using Unity.Netcode;

namespace Door
{
    public class MyDoorController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Animator doorAnimator;
        [SerializeField] public bool doorOpen = false;
        NavMeshSurface _surface;

        #endregion

        private void Start() {
            _surface = GameObject.FindGameObjectWithTag("Platform").GetComponent<NavMeshSurface>();
        }

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

            StartCoroutine(ReBake());
        }

        public bool AnimatorIsPlaying()
        {
            return doorAnimator.GetCurrentAnimatorStateInfo(0).length >
                   doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        private IEnumerator ReBake()
        {
            yield return new WaitForSeconds(1);
            SurfaceBakeServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayAnimationServerRpc(string animationName)
        {
            doorAnimator.Play(animationName, 0, 0.0f);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SurfaceBakeServerRpc()
        {
            _surface.BuildNavMesh();
        }

    }
}
