using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //Intro Sequence gameObject has Playable Director and this using is related to it
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        public bool alreadyTriggered = false;


        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.gameObject.tag == "Player")
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        #region Saveable Interface
        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;

        }
        #endregion
    }

}

