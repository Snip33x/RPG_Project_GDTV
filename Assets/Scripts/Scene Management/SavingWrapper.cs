using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

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
        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        private void Load()
        {
            //call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }


    }
}

