using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//avoiding singleton Pattern
namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false; //static variable lives in aplication, it won't change like other when we instantiate object for example

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects(); //it is not accesing a gameObject before awake so we are safe

            hasSpawned = true;
            //now our fader won't be reactivated every time we use portal when we had coroutine inside Fader to do so -early dev of fader
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}