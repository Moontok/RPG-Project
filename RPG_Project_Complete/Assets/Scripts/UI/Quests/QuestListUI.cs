using UnityEngine;
using RPG.Quests;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab = null;
        [SerializeField] Button activeButton = null;
        [SerializeField] Button completedButton = null;

        QuestList questList = null;
        bool activeQuestTab = true;

        void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onUpdate += Redraw;
            Redraw();
        }

        public void DisplayActiveQuests(bool state)
        {
            activeQuestTab = state;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            
            if (activeQuestTab)
            {
                activeButton.interactable = false;
                completedButton.interactable = true;
                foreach (QuestStatus status in questList.GetStatuses())
                {
                    if (!status.IsComplete())
                    {
                        QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, this.transform);
                        uiInstance.Setup(status);
                    }
                }
            }
            else
            {
                activeButton.interactable = true;
                completedButton.interactable = false;
                foreach (QuestStatus status in questList.GetStatuses())
                {
                    if (status.IsComplete())
                    {
                        QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, this.transform);
                        uiInstance.Setup(status);
                    }
                }
            }
        }
    }
}
