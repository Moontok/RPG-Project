using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PRG.UI
{    
    public class SaveLoadUI : MonoBehaviour 
    {
        [SerializeField] Transform contentRoot = null;
        [SerializeField] GameObject rowPrefab = null;

        void OnEnable()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) { return; }

            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
            foreach (string save in savingWrapper.ListSaves())
            {
                GameObject rowInstance = Instantiate(rowPrefab, contentRoot);
                Button loadButton = rowInstance.GetComponent<LoadButtonSetup>().GetLoadButton();
                Button deleteButton = rowInstance.GetComponent<LoadButtonSetup>().GetDeleteButton();

                TMP_Text textComp = loadButton.GetComponentInChildren<TMP_Text>();
                textComp.text = save;

                loadButton.onClick.AddListener(() => {
                    savingWrapper.LoadGame(save);
                });
                deleteButton.onClick.AddListener(() =>{
                    savingWrapper.DeleteSavedGame(save);
                    Destroy(rowInstance);
                });
            }
        }
    }
}