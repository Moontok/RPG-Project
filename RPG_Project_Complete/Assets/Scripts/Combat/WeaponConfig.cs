using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make NewWeapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        //[SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageDamageBonus = 0f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weapopnAttackSpeed = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        //Added        
        [SerializeField]
        Modifiers[] additiveModifiers = null;
        [SerializeField]
        Modifiers[] percentageModifiers = null;

        [System.Serializable]
        struct Modifiers
        {
            public Stat stat;
            public float value;
        }

        float weaponDamage = 0f;
        //End Adding

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;            
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null) { oldWeapon = leftHand.Find(weaponName); }
            if (oldWeapon == null ) { return; }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform = null;
            if (isRightHanded) { handTransform = rightHand; }
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageDamageBonus()
        {
            return percentageDamageBonus;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetAttackSpeed()
        {
            return weapopnAttackSpeed;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
            
        }        

        // public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        // {
        //     if (stat == Stat.Damage)
        //     {
        //         yield return weaponDamage;
        //     }
        // }

        // public IEnumerable<float> GetPercentageModifiers(Stat stat)
        // {
        //     if (stat == Stat.Damage)
        //     {
        //         yield return percentageDamageBonus;
        //     }
        // }
    }
}
