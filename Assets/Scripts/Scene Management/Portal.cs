using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }

        [SerializeField] int sceneToLoad = -1; // we will remember to change it in the inspector to right one
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        GameObject player;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            player = GameObject.FindWithTag("Player");

            DontDestroyOnLoad(gameObject);  //dont destroy the portal until the new world has loaded up
            
            Fader fader = FindObjectOfType<Fader>();
            //save current level
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>(); //saving wrapper is in hierarchy Core->PersistentObject Prefab-> Saving Children
            // remove control
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);  //Unity knows that it needs to run this coroutine once the scene is loaded
            PlayerController newplayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); //new player in new scene
            newplayerController.enabled = false;


            // Load current level
            savingWrapper.Load();
            

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal); //updating player position

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime); //wait for Camera to stabilize
            fader.FadeIn(fadeInTime);


            newplayerController.enabled = true;
            Destroy(gameObject); //job of this current Portal is done so we destroy it
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue; //We dont want this same exact portal
                if (portal.destination != this.destination) continue; //returning only when portal has right destination
                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            player = GameObject.FindWithTag("Player");
            //if our character is spawning at wrong locations reenable navMeshAgent
            player.GetComponent<NavMeshAgent>().enabled = false;  // or use .Warp
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }



    }
}