using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            //%hp GetComponent<Text>().text = String.Format("{0:0}%",health.GetPercentage()); //take first thing on ther right - health.GetPercentage() and put it into a place where is {0}, u can add {1} for example , 0:0 - format that value, and give 0 decimal - 0:0.1 - give 1 decimal
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHeatlhPoints());
        }
    }

}