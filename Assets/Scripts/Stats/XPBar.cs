using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class XPBar : MonoBehaviour
    {
        [SerializeField] Slider xpBar;
        Experience experience;
        BaseStats playerStats;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {
            xpBar.value = experience.GetExperience() / playerStats.GetStat(Stat.ExperienceToLevelUp);
        }
    }

}
