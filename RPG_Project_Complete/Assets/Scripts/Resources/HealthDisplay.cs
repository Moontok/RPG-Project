using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health = null;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            this.GetComponent<Text>().text = $"{health.GetHealthPoints():0}/{health.GetMaxHealthPoints():0}";
        }
    }
}
