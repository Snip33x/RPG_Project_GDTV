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
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        Health target; //!!!! FIND OUT how this target is set up-- its dont in Attack method:)  //we changed it from transform to health to be more specific , no need to getcomponent now
        float timeSinceLastAttack = Mathf.Infinity; //before we had 0, and it took long time for our character to attack at start

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;


            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange()) //if (target != null && !GetIsInRange()) IMPORTANT NOTE  && operator, if the first thing is false it skips evaluating the second because the result will alwyas be false -- optimalization! - this way we got rid of null reference error -- eventualy we had a problem here that we were stopping character all the time, so return early //quiz Bacis Combat #1
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f); //1f= moving at full speed
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()  
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) // throttling attack , making it slower
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack"); // Removin BUG that causes our character to attack - stop attack glitch, when we canceled attack before and moved away, test higher timebetweenattacks to see clearly
            GetComponent<Animator>().SetTrigger("attack"); //setting up attack animatior
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }
            target.TakeDamage(weaponDamage); // removing get component and turing it into this line
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange; //we are not using navmeshagent.remainingdistance because it is not calculating real distance, - as remainingDistance is a calculation what what actually needs to be travelled to get to the location.
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //print("Take that you short, squat peasant!");
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)  //making our raycast not hit dead bodies -- using this method in player Controller
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponOverride;
        }


        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack"); //trigger in transition to make our character immidietly stop attacking animation when we move
        }
    }

}