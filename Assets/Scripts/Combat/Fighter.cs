using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;

namespace RPG.Combat
{    
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;



        private void Update()
        {
            if (target == null) return;

            if (!GetIsInRange()) //if (target != null && !GetIsInRange()) IMPORTANT NOTE  && operator, if the first thing is false it skips evaluating the second because the result will alwyas be false -- optimalization! - this way we got rid of null reference error -- eventualy we had a problem here that we were stopping character all the time, so return early 
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange; //we are not using navmeshagent.remainingdistance because it is not calculating real distance, - as remainingDistance is a calculation what what actually needs to be travelled to get to the location.
        }

        public void Attack(CombatTarget combatTarget)
        {
            //print("Take that you short, squat peasant!");
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }

}