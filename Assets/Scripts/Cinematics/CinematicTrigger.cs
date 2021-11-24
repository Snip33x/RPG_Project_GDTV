using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //Intro Sequence gameObject has Playable Director and this using is related to it
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        public bool alreadyTriggeredCinematic = false;


        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggeredCinematic && other.gameObject.tag == "Player")
            {
                alreadyTriggeredCinematic = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        #region Saveable Interface
        public object CaptureState()
        {
            return alreadyTriggeredCinematic;
        }

        public void RestoreState(object state)
        {
            alreadyTriggeredCinematic = (bool)state;

        }
        #endregion
    }

}

