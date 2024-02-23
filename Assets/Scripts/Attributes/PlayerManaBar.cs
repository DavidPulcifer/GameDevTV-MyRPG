using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerManaBar : MonoBehaviour
    {
        [SerializeField] Mana playerMana;
        [SerializeField] Slider manaBar;

        // Update is called once per frame
        void Update()
        {
            manaBar.value = playerMana.GetManaFraction();
        }
    }
}
