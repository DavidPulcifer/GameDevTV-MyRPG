using GameDevTV.Utils;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] bool respawn = false;
        [Min(-1)]
        [Tooltip("Number of times to respawn. -1 is infinite")]
        [SerializeField] int respawnNumber = 0;
        [SerializeField] float respawnDelay = 1f;
        [SerializeField] TakeDamageEvent takeDamage;
        public UnityEvent onDie;
        int respawnsRemaining;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        
        LazyValue<float> healthPoints;
        bool wasDeadLastFrame = false;

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            respawnsRemaining = respawnNumber;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();            
        }

        public bool IsDead() { return healthPoints.value <= 0; }

        public float GetHealthPoints() { return healthPoints.value; }

        public float GetMaxHealthPoints() { return GetComponent<BaseStats>().GetStat(Stat.Health); }

        public void TakeDamage(GameObject instigator, float damage)
        {            
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (IsDead())
            {                
                onDie.Invoke();
                AwardExperience(instigator);
                if(respawn && (respawnsRemaining > 0 || respawnNumber == -1))
                {
                    StartCoroutine(Respawn());
                }
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            UpdateState();
        }
        
        public void Heal(float amountToHeal)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amountToHeal, GetMaxHealthPoints());
            UpdateState();
        }

        private IEnumerator Respawn()
        {            
            yield return new WaitForSeconds(respawnDelay);

            // Revive the character
            Heal(GetMaxHealthPoints());

            if (respawnsRemaining > 0)
            {
                respawnsRemaining--;
            }                            
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if(!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if(wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        void RegenerateHealth()
        {
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            UpdateState();            
        }

    }
}

