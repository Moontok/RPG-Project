using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utils;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

// TODO: Debug mode
        [SerializeField] float autoAttackRange = 4f;
// End Debug Mode
        Health target = null;
        Equipment equipment = null;
        float timeSinceLastAttack = Mathf.Infinity;

        WeaponConfig currentWeaponConfig = null;
        LazyValue<Weapon> _currentWeapon = null;
        public Weapon currentWeapon
        {
            get {return _currentWeapon.value;}
            set {_currentWeapon.value = value;}
        }
        

        private void Awake() 
        {
            currentWeaponConfig = defaultWeapon;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = this.GetComponent<Equipment>();
            if (equipment)
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
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) { return; }
            // Normal mode: no auto attack
            //if(target.IsDead()) { return; }
            
// TODO: Debug mode: Auto attack 
            if (target.IsDead())
            {
                target = FindNewTargetInRange();
                if (target == null) { return; }
            }
// End Debug Mode

            if (!GetIsInRange(target.transform))
            {
                this.GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                this.GetComponent<Mover>().Cancel();                
                AttackBehavior();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon = AttachWeapon(weapon);
        }

        public Health GetTarget()
        {
            return target;
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand) { return rightHandTransform; }
            else { return leftHandTransform; }
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
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
            Animator animator = this.GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehavior()
        {
            this.transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeaponConfig.GetAttackSpeed())
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            this.GetComponent<Animator>().ResetTrigger("stopAttack");
            this.GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;

            float damage = this.GetComponent<BaseStats>().GetStat(Stat.Damage);
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();
            if (targetBaseStats != null)
            {
                float defense = targetBaseStats.GetStat(Stat.Defense);
                damage /= 1 + defense / damage;
            }

            if (currentWeapon != null) currentWeapon.OnHit();

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, this.gameObject, damage);                
            }
            else
            {
                target.TakeDamage(this.gameObject, damage);
            }
        }
        // Animation Event on Bow
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(this.transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!this.GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            this.GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            this.GetComponent<Mover>().Cancel();            

        }

        private void StopAttack()
        {
            this.GetComponent<Animator>().ResetTrigger("attack");
            this.GetComponent<Animator>().SetTrigger("stopAttack");
        }

// TODO: Debug mode
        private Health FindNewTargetInRange()
        {
            Health best = null;
            float bestDistance = Mathf.Infinity;

            foreach (var candidate in FindAllTargetsInRange())
            {
                float candidateDistance = Vector3.Distance(this.transform.position, candidate.transform.position);
                if (candidateDistance < bestDistance)
                {
                    best = candidate;
                    bestDistance = candidateDistance;
                }
            }
            return best;
        }

        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(this.transform.position, autoAttackRange, Vector3.up, 0);

            foreach (var hit in raycastHits)
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health == null) { continue; }
                if (health.IsDead()) { continue; }
                if (health.gameObject == this.gameObject) { continue; }
                yield return health;
            }

        }
// End Debug Mode
    }
}
