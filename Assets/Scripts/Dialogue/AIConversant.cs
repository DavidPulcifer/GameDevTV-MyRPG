using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName;
       

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) return false;

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = (callingController.transform.position - transform.position).normalized;
                Vector3 targetPosition = transform.position + direction * 1f; // 1 unit away in the direction of callingController
                callingController.GetComponent<Mover>().StartMoveAction(targetPosition, 1f);
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }
            return true;
        }

        public string GetName() { return conversantName; }

        
    }
}
