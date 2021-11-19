using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

//OBSERVER PATTERN

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; //played is event  ctrl+mouse on played
            GetComponent<PlayableDirector>().stopped += EnableControl; //stopped is event
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector pd) //argument must be putted here becasue in PlayabeDirectrol class -- public event Action<PlayableDirector> played;  -- in Action<> there is PlayableDirector
        {       
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

