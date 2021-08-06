using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour 
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> intialCooldownTimes
         = new Dictionary<InventoryItem, float>();

        void Update()
        {
            List<InventoryItem> keys = new List<InventoryItem>(cooldownTimers.Keys);
            foreach (InventoryItem ability in keys)
            {
                cooldownTimers[ability] -= Time.deltaTime;
                if (cooldownTimers[ability] < 0)
                {
                    cooldownTimers.Remove(ability);
                    intialCooldownTimes.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItem ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            intialCooldownTimes[ability] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem ability)
        {
            if (!cooldownTimers.ContainsKey(ability)) { return 0; }

            return cooldownTimers[ability];
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null) { return 0; }

            if (!cooldownTimers.ContainsKey(ability)) { return 0; }

            return cooldownTimers[ability] / intialCooldownTimes[ability];
        }
    }    
}