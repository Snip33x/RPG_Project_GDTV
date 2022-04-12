using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour //we use this to drive our saving system
    {
        const string defaultSaveFile = "save";  //SavingWrapper is that if we use it in different project we can choose different place to save, or slot
        [SerializeField] float fadeInTime = 0.2f;


        private void Awake() // we had bug where lvl wasn't updated when we saved, and then loaded game so we call this first, we restore state, and then call every start
        {
            StartCoroutine(LoadLastScene());
        }
        IEnumerator LoadLastScene() //it calls start as a coroutine
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Delete();
            }

        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            //call to saving system load
            StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

    }
}

