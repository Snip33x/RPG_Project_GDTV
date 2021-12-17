using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if(fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            Health health = fighter.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage()); //take first thing on ther right - health.GetPercentage() and put it into a place where is {0}, u can add {1} for example , 0:0 - format that value, and give 0 decimal - 0:0.1 - give 1 decimal
        }
    }

}