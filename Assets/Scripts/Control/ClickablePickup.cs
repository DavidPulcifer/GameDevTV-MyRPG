using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = (callingController.transform.position - transform.position).normalized;
                Vector3 targetPosition = transform.position + direction * 1f; // 1 unit away in the direction of callingController
                callingController.GetComponent<Mover>().StartMoveAction(targetPosition, 1f);
                pickup.PickupItem();
            }
            return true;
        }
    }
}
