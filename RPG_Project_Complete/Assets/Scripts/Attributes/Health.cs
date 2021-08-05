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
        [SerializeField] UnityEvent onDie = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {            
        }

        LazyValue<float> _healthPoints = null;
        public float healthPoints
        {
            get { return _healthPoints.value; }
            set { _healthPoints.value = value; }
        }

        bool isDead = false;

        private void Awake() 
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return this.GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() 
        {
            _healthPoints.ForceInit();
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
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            takeDamage.Invoke(damage);
            if (healthPoints == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPoints = Mathf.Min(healthPoints + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return healthPoints;
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
            return healthPoints / this.GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if(isDead) return;

            isDead = true;
            this.GetComponent<Animator>().SetTrigger("die");
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience(this.GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = this.GetComponent<BaseStats>().GetStat(Stat.Health) * (regenPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
