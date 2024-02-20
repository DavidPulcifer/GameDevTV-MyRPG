using RPG.Attributes;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        [SerializeField] Trait modifyingTrait = Trait.Strength;
        [SerializeField] float additiveTraitModifier = 0;
        [SerializeField] float percentageTraitModifier = 0;

        public override void StartEffect(AbilityData data, Action finished)
        {
            
            int traitPoints = GameObject.FindWithTag("Player").GetComponent<TraitStore>().GetPoints(modifyingTrait);
            healthChange = (healthChange + (additiveTraitModifier * traitPoints)) * (1 + ((percentageTraitModifier * traitPoints) / 100));
            
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if(healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }                    
                }
            }
            finished();
        }
    }
}


