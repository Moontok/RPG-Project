using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
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
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public void Delete()
        {
            this.GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
