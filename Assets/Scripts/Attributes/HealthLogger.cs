using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthLogger : MonoBehaviour
    {
        [SerializeField] Health health;
                
        void Update()
        {
            Debug.Log(health.GetMaxHealthPoints());
            Debug.Log(health.GetHealthPoints());
        }
    }

}
