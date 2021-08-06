using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        LazyValue<float> manaPoints = null;

        void Awake() 
        {
            this.manaPoints = new LazyValue<float>(GetMaxManaPoints);
        }

        void Update() 
        {
            if (manaPoints.value < GetMaxManaPoints())
            {
                manaPoints.value += GetManaRegenRate() * Time.deltaTime;
                if (manaPoints.value > GetMaxManaPoints())
                {
                    manaPoints.value = GetMaxManaPoints();
                }
            }
        }

        public float GetManaPoints()
        {
            return manaPoints.value;
        }

        public float GetMaxManaPoints()
        {
            return this.GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetManaRegenRate()
        {
            return this.GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);

        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > manaPoints.value) { return false; }

            manaPoints.value -= manaToUse;
            return true;
        }        

        public object CaptureState()
        {
            return manaPoints.value;
        }

        public void RestoreState(object state)
        {
            manaPoints.value = (float) state;
        }
    }    
}