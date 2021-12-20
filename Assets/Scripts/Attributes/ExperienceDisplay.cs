using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text =  experience.GetExperiencePoint().ToString(); //take first thing on ther right - health.GetPercentage() and put it into a place where is {0}, u can add {1} for example , 0:0 - format that value, and give 0 decimal - 0:0.1 - give 1 decimal
        }
    }

}