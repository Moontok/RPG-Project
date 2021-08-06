using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana = null;

        private void Awake()
        {
            this.mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }

        private void Update()
        {
            this.GetComponent<Text>().text = $"{mana.GetManaPoints():0}/{mana.GetMaxManaPoints():0}";
        }
    }
}
