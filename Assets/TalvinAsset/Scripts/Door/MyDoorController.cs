using System.Collections;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

namespace Door
{
    public class MyDoorController : NetworkBehaviour
    {
        #region Variables

        private NavMeshSurface _surface;
        public NetworkVariable<bool> isLocked = new NetworkVariable<bool> (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [SerializeField] private Animator doorAnimator;
        [SerializeField] public bool doorOpen;

        #endregion

        private void Start()
        {
            gameObject.tag = isLocked.Value ? "Locked" : "Unlocked";
            isLocked.OnValueChanged += ChangeDoorTag;
            _surface = GameObject.FindGameObjectWithTag("Platform").GetComponent<NavMeshSurface>();
        }

        private void ChangeDoorTag(bool previous, bool newValue){
            Debug.Log("In change door tag delegate");
            if (isLocked.Value == false)
            {
                ChangeTagServerRpc();
            }
        }

        public void PlayAnimation()
        {
            if (!doorOpen)
            {
                PlayAnimationServerRpc("DoorOpen");
                doorOpen = true;
            }

            else
            {
                PlayAnimationServerRpc("DoorClose");
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

        [ServerRpc(RequireOwnership = false)]
        private void ChangeTagServerRpc()
        {
            ChangeTagClientRpc();
        }

        [ClientRpc]
        private void ChangeTagClientRpc()
        {
            gameObject.tag = "Unlocked";
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeVariableServerRpc(bool value){
            isLocked.Value = value;
            Debug.Log("value" + isLocked.Value);
        }
    }
}
