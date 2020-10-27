using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{    
    [CreateAssetMenu(fileName = "Quests", menuName = "RPG_Project_Complete/Quests", order = 0)]
    public class Quest : ScriptableObject 
    {
        [SerializeField] string[] objectives = null;

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Length;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }
    }
}
