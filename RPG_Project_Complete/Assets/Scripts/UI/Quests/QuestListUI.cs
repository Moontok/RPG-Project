using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab = null;
        QuestList questList = null;

        void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (QuestStatus status in questList.GetStatuses())
            {
                if (!status.IsComplete())
                {
                    QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, this.transform);
                    uiInstance.Setup(status);
                }
            }
        }
    }
}
