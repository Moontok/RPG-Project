using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Start()
        {
            
        }

        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Load();
            }
        }

        public void Load()
        {
            this.GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            this.GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
