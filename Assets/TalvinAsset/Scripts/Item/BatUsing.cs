using System;
using System.Collections;
using Unity.Netcode;
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

        private GameObject _playerHitGameObject;

        private void Update()
        {
            if (!gameObject.activeSelf) return;
            Ray ray = new Ray(controller.transform.position + new Vector3(0,1,0), controller.transform.forward);
            if (!Physics.Raycast(ray, out var playerHit, maxDistance, playerMask) || !Input.GetKeyDown(userInputs.attack))
                return;

            _playerHitGameObject = playerHit.transform.gameObject;

            if (_playerHitGameObject.TryGetComponent(out PlayerNetwork player))
            {
                player.ChangeCanMoveCameraServerRpc((int)player.GetComponent<NetworkObject>().NetworkObjectId);
                return;
            }

            throw new Exception();
        }

        private void OnDrawGizmos()
        {
            if(!gameObject.activeSelf) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(controller.transform.position + new Vector3(0,1,0), controller.transform.forward);
        }
    }
}
