using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] KeyCode saveKey = KeyCode.F5;
        [SerializeField] KeyCode loadKey = KeyCode.F9;
        [SerializeField] KeyCode deleteKey = KeyCode.Delete;
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeWaitTime = 1f;

        private void Awake() 
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return this.GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() 
        {
            if (Input.GetKeyDown(saveKey))
            {
                Save();
            }
            if (Input.GetKeyDown(loadKey))
            {
                ManualLoad();
            }
            if (Input.GetKeyDown(deleteKey))
            {
                Delete();
            }
        }

        public void Load()
        {
            this.GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void ManualLoad()
        {
            StartCoroutine(LoadLastScene());
        }

        public void Save()
        {
            this.GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            this.GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
