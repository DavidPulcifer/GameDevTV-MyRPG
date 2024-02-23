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
        [SerializeField] float waitTime = 0.25f;

        PlayerConversant playerConversant;

        private void Awake()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();            
        }

        private void Start()
        {
            Invoke("CheckForAggroActivation", waitTime);
        }

        void CheckForAggroActivation()
        {
            aggroGroup.Activate(playerConversant.HasDialogueFlag(aggroFlag));
        }
    }
}
