using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title = null;
        [SerializeField] Transform objectiveContainer = null;
        [SerializeField] GameObject objectivePrefab = null;
        [SerializeField] GameObject objectiveIncompletePrefab = null;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
                        
            foreach (Transform child in objectiveContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if(status.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }
        }
    }
}
