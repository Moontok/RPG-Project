using RPG.SceneManagement;
using RPG.Utils;
using UnityEngine;
using TMPro;

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

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else 
            Application.Quit();
#endif
        }

        private SavingWrapper GetSavingWrapper()
        {            
            return FindObjectOfType<SavingWrapper>();
        }
    }    
}
