using RPG.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;

        private void Start()
        {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                AIConversant aiConversant = fighter.GetComponent<AIConversant>();
                                
                if (target != null) target.enabled = shouldActivate;
                if (aiConversant != null) aiConversant.enabled = !shouldActivate;

                fighter.enabled = shouldActivate;
            }
        }
    }

}