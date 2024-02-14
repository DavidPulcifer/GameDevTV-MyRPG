using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName;
        Vector3 startingPosition;
       

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public void ResetPosition()
        {
            Mover mover = GetComponent<Mover>();
            if(mover)
            {
                mover.StartMoveAction(startingPosition, 1f);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) return false;

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = (callingController.transform.position - transform.position).normalized;
                Vector3 targetPosition = transform.position + direction * 2f; // 1 unit away in the direction of callingController
                Vector3 npcTargetPosition = transform.position + direction * 1f;
                startingPosition = transform.position;
                Mover npcMover = GetComponent<Mover>();
                if(npcMover) npcMover.StartMoveAction(npcTargetPosition, 1f);
                callingController.GetComponent<Mover>().StartMoveAction(targetPosition, 1f);
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }
            return true;
        }        

        public string GetName() { return conversantName; }

        
    }
}
