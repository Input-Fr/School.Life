using System;
using System.Collections;
using UnityEngine;
using User;

namespace Item
{
    public class BatUsing : MonoBehaviour
    {
        [SerializeField] private UserInputs userInputs;
        [SerializeField] private GameObject controller;

        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask playerMask;

        private GameObject _playerHit;

        private void Update()
        {
            if (!gameObject.activeSelf) return;
            Ray ray = new Ray(controller.transform.position + new Vector3(0,1,0), controller.transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, playerMask))
                return;

            Debug.Log("Player in view");
            _playerHit = hit.transform.gameObject;

            StartCoroutine(UpdateCameraMovement(_playerHit));
        }

        private IEnumerator UpdateCameraMovement(GameObject playerHit)
        {
            Debug.Log("set camera move false");
            playerHit.GetComponent<vThirdPersonCamera>().SetCanMoveCamera(false);
            
            yield return new WaitForSeconds(5f);
            playerHit.GetComponent<vThirdPersonCamera>().SetCanMoveCamera(true);
            _playerHit = null;
        }

        private void OnDrawGizmos()
        {
            if(!gameObject.activeSelf) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(controller.transform.position + new Vector3(0,1,0), controller.transform.forward);
        }
    }
}
