using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] float autoAttackRange = 4f;
        [SerializeField] bool autoAttackActive = true;

        Health target;
        Equipment equipment;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if(equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {            
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }        

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead())
            {
                if(autoAttackActive) target = FindNewTargetInRange();
                if (target == null) return;
            }
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if(weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() { return target; }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand) return rightHandTransform;
            return leftHandTransform;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            //This animation will trigger Hit() as an animation event
            TriggerAttack();
            timeSinceLastAttack = 0;
        }

        private Health FindNewTargetInRange()
        {
            Health bestCandiate = null;
            float bestCandidateDistance = Mathf.Infinity;
            foreach (var candidate in FindAllTargetsInRange())
            {
                float candidateDistance = Vector3.Distance(transform.position,
                                                            candidate.transform.position);
                if(candidateDistance < bestCandidateDistance)
                {
                    bestCandiate = candidate;
                    bestCandidateDistance = candidateDistance;
                }
            }
            return bestCandiate;
        }

        IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position,
                                                                autoAttackRange,
                                                                Vector3.up);
            foreach (var hit in raycastHits)
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health == null) continue;
                if (health.IsDead()) continue;
                if (health.gameObject == gameObject) continue;
                yield return health;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();
            if (targetBaseStats != null)
            {
                float defense = targetBaseStats.GetStat(Stat.Defense);
                damage /= 1 + defense / damage;
            }

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                
                target.TakeDamage(gameObject, damage);
            }            
        }

        //Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return ((transform.position - target.transform.position).sqrMagnitude < (currentWeaponConfig.GetWeaponRange() * currentWeaponConfig.GetWeaponRange()));
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) { return false; }

            Health targetToTest = combatTarget.GetComponent<Health>();

            return (targetToTest != null) && !targetToTest.IsDead();

        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }        
    }
}
