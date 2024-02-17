using RPG.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroState : MonoBehaviour
    {
        [SerializeField] AggroGroup aggroGroup;
        [SerializeField] string aggroFlag;

        PlayerConversant playerConversant;

        private void Awake()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            aggroGroup.Activate(playerConversant.HasDialogueFlag(aggroFlag));
        }        
    }
}
