using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class TargetHealthDisplay : MonoBehaviour
    {
        Fighter fighter = null;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {

            Health targetHealth = fighter.GetTarget();
            if (targetHealth != null) this.GetComponent<Text>().text = $"{targetHealth.GetHealthPoints():0}/{targetHealth.GetMaxHealthPoints():0}";
            else this.GetComponent<Text>().text = "No Target";
        }
    }
}
