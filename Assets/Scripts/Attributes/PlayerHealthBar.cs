using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] Health playerHealth;
        [SerializeField] Slider hpBar;

        void Update()
        {
            hpBar.value = playerHealth.GetFraction();
        }
    }
}
