using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoint = 0;
        public float GetExperiencePoint()
        {
            return experiencePoint;
        }
        public void GainExperience(float experience)
        {
            experiencePoint += experience;
        }
        
        #region Saveable Interface
        public object CaptureState()
        {
            return experiencePoint;
        }
        public void RestoreState(object state)
        {
            experiencePoint = (float)state;
        }
        #endregion
    }

}