using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "My Ability", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy = null;
        [SerializeField] FilterStrategy[] filterStrategies = null;
        [SerializeField] EffectStrategy[] effectStrategies = null;
        [SerializeField] float cooldownTime = 0f;
        [SerializeField] float manaCost = 0f;

        public override bool Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();
            if (mana.GetManaPoints() < manaCost) { return false; }

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) { return false; }

            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () => TargetAcquired(data));

            return true;
        }

        private void TargetAcquired(AbilityData data)
        {
            if (data.IsCancelled()) { return; }

            Mana mana = data.GetUser().GetComponent<Mana>();
            if (!mana.UseMana(manaCost)) { return; }

            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));                
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {

        }
    }
}
