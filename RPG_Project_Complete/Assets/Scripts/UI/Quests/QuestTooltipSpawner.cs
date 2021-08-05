using RPG.Quests;
using RPG.Utils.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = this.GetComponent<QuestItemUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(status);
        }
    }
}
