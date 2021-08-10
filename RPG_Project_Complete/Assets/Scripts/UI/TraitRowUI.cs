using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] Trait trait = Trait.Strength;
        [SerializeField] TextMeshProUGUI valueText = null;
        [SerializeField] Button minusButton = null;
        [SerializeField] Button plusButton = null;

        TraitStore playerTraitStore = null;

        void Start() 
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(1));
        }

        void Update() 
        {
            minusButton.interactable = playerTraitStore.CanAssignPoints(trait, -1);
            plusButton.interactable = playerTraitStore.CanAssignPoints(trait, 1);

            valueText.text = $"{playerTraitStore.GetProposedPoints(trait)}";
        }

        public void Allocate(int points)
        {
            playerTraitStore.AssignPoints(trait, points);
        }
    }
}
