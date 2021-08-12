using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {        
        const string currentSaveKey = "currentSaveName";

        [SerializeField] KeyCode saveKey = KeyCode.F5;
        [SerializeField] KeyCode loadKey = KeyCode.F9;
        [SerializeField] KeyCode deleteKey = KeyCode.Delete;

        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;

        public void ContinueGame() 
        {
            if (String.IsNullOrEmpty(GetCurrentSave())) { return; }
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            if (String.IsNullOrEmpty(saveFile)) { return; }
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        public void DeleteSavedGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            Delete();
            SetCurrentSave("");
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
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

        public void RestorePlayerAfterSceneChange()
        {
            this.GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void ManualLoad()
        {
            StartCoroutine(LoadLastScene());
        }

        public void Save()
        {
            this.GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            this.GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
