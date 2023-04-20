using System;
using System.Collections;
using Inventory;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace Item
{
    public class BatUsing : MonoBehaviour
    {
        [HideInInspector] public InventoryItem inventoryItem;
        private int _numberKickLeft;
        private Image _toFill;

        [SerializeField] private GameObject fillLeftKickAmountPrefab;

        [SerializeField] private int numberKick;
        [SerializeField] private BatState batState;
        
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
                _numberKickLeft--;
                UpdateFillAmount();

                if (_numberKickLeft <= 0)
                {
                    batState.DestroyBat();
                }

                return;
            }

            throw new Exception();
        }

        public void InstantiateFillAmount()
        {
            _numberKickLeft = numberKick;
            _toFill = Instantiate(fillLeftKickAmountPrefab, inventoryItem.transform).GetComponent<Image>();
            _toFill.fillAmount = Mathf.InverseLerp(0, numberKick, _numberKickLeft);
        }

        private void UpdateFillAmount()
        {
            _toFill.fillAmount = Mathf.InverseLerp(0, numberKick, _numberKickLeft);
        }

        private void OnDrawGizmos()
        {
            if(!gameObject.activeSelf) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(controller.transform.position + new Vector3(0,1,0), controller.transform.forward);
        }
    }
}
