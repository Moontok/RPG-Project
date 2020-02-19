using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                this.GetComponent<SavingSystem>().Save(defaultSaveFile);
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                this.GetComponent<SavingSystem>().Load(defaultSaveFile);
            }
        }
    }
}
