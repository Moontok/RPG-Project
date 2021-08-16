using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;

namespace PRG.UI
{    
    public class PauseMenuUI : MonoBehaviour 
    {
        PlayerController playerController = null;

        void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        void OnEnable() 
        {
            Time.timeScale = 0;
            playerController.enabled = false;
            playerController.ResetCursor();
        }

        void OnDisable() 
        {
            // Change back to 1 or 5 for testing
            Time.timeScale = 5;
            playerController.enabled = true;
        }

        public void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
    }
}