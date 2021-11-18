using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//OBSERVER PATTERN

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; //played is event  ctrl+mouse on played
            GetComponent<PlayableDirector>().stopped += EnableControl; //stopped is event
        }

        void DisableControl(PlayableDirector pd) //argument must be putted here becasue in PlayabeDirectrol class -- public event Action<PlayableDirector> played;  -- in Action<> there is PlayableDirector
        {
            print("DisableControl");
        }

        void EnableControl(PlayableDirector pd)
        {
            print("EndbleControl");
        }
    }
}

