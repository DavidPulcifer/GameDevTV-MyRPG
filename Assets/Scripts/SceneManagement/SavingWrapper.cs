using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstSceneBuildIndex = 1;
        [SerializeField] int menuSceneBuildIndex = 0;

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveName)
        {
            if (String.IsNullOrEmpty(saveName)) return;
            SetCurrentSave(saveName);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveName)
        {
            SetCurrentSave(saveName);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        void SetCurrentSave(string saveName)
        {
            PlayerPrefs.SetString(currentSaveKey, saveName);
        }


        IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstSceneBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuSceneBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadLastScene()
        {            
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {            
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }

}