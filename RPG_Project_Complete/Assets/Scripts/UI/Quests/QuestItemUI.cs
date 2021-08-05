﻿using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title = null;
        [SerializeField] TextMeshProUGUI progress = null;

        QuestStatus status = null;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.GetQuest().GetTitle();
            progress.text = $"{status.GetCompletedCount()}/{status.GetQuest().GetObjectiveCount()}";
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
