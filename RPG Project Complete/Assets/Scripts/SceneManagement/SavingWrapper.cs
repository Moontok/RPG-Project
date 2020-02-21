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

        private IEnumerator Start()
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
