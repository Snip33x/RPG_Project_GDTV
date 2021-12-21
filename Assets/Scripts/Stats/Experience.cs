using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate(); -- this is just Action, otherwise, we would need this line plus -- pubilc event ExperienceGainedDelegate on EcperienceGained - second line
        public event Action onExperienceGained;

        public void GainExperience(float experience) //this is called in health script, and event Action onExperienceGained is called, and in base stats we are calling/subscribint UpdateLevel to it
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        

        public float GetExperiencePoint()
        {
            return experiencePoints;
        }
        #region Saveable Interface
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            //print("experience load done");
            experiencePoints = (float)state;
        }
        #endregion
    }

}