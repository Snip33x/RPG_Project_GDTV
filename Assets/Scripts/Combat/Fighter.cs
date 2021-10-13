using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Combat
{    
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Transform target; //!!!! FIND OUT how this target is set up
        float timeSinceLastAttack = 0;


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;


            if (target == null) return;

            if (!GetIsInRange()) //if (target != null && !GetIsInRange()) IMPORTANT NOTE  && operator, if the first thing is false it skips evaluating the second because the result will alwyas be false -- optimalization! - this way we got rid of null reference error -- eventualy we had a problem here that we were stopping character all the time, so return early //quiz Bacis Combat #1
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()  
        {
            if (timeSinceLastAttack > timeBetweenAttacks) // throttling attack , making it slower
            {
                // This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack"); //setting up attack animatior
                timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange; //we are not using navmeshagent.remainingdistance because it is not calculating real distance, - as remainingDistance is a calculation what what actually needs to be travelled to get to the location.
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //print("Take that you short, squat peasant!");
            target = combatTarget.transform;
        }


        public void Cancel()
        {
            target = null;
        }


    }

}