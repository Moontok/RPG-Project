using RPG.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenPercentage = 100f;
        [SerializeField] TakeDamageEvent takeDamage = null;
        public UnityEvent onDie = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {            
        }

        LazyValue<float> healthPoints = null;

        bool wasDeadLastFrame = false;

        private void Awake() 
        {
            healthPoints = new LazyValue<float>(GetMaxHealthPoints);
        }

        private void Start() 
        {
            healthPoints.ForceInit();
        }

        private void OnEnable() 
        {
            this.GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() 
        {
            this.GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (IsDead())
            {
                onDie.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return this.GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());
        }

        public float GetFraction()
        {
            return healthPoints.value / this.GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void UpdateState()
        {            
            Animator animator = this.GetComponent<Animator>();

            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                this.GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }
            
            experience.GainExperience(this.GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = this.GetComponent<BaseStats>().GetStat(Stat.Health) * (regenPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            UpdateState();
        }
    }
}
