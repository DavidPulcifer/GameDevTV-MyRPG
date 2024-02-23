using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unassignedPointsText;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] Button commitButton;

        TraitStore playerTraitStore = null;
        BaseStats baseStats;

        private void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            commitButton.onClick.AddListener(playerTraitStore.Commit);
        }

        private void Update()
        {
            unassignedPointsText.text = playerTraitStore.GetUnassignedPoints().ToString();
            levelText.text = baseStats.GetLevel().ToString();
        }
    }

}
