using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter playerFighterComponent = null;

        private void Awake()
        {            
            playerFighterComponent = GameObject.FindWithTag("Player").GetComponent<Fighter>();            
        }

        private void Update()
        {
            if(playerFighterComponent.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }

            Health health = playerFighterComponent.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());


        }
    }
}
