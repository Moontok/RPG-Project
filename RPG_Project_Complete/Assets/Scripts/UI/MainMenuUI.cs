using RPG.SceneManagement;
using RPG.Utils;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField newGameNameField = null;

        LazyValue<SavingWrapper> savingWrapper = null;

        void Awake() 
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }    
}
