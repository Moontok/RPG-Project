using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unassignedPointsText = null;
        [SerializeField] Button commitButton = null;

        TraitStore playerTraitStore = null;

        void Start() 
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            commitButton.onClick.AddListener(playerTraitStore.Commit);
        }

        void Update() 
        {
            unassignedPointsText.text = $"{playerTraitStore.GetUnassignedPoints()}";
        }
    }    
}
