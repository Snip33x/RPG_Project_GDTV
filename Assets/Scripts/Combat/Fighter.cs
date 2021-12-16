using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{    
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target; //!!!! FIND OUT how this target is set up-- its not in Attack method:)  //we changed it from transform to health to be more specific , no need to getcomponent now
        float timeSinceLastAttack = Mathf.Infinity; //before we had 0, and it took long time for our character to attack at start
        Weapon currentWeapon = null;

        private void Start()
        {   
            if(currentWeapon == null) //defend when loading into game, the saved weapon will come first
            {
                EquipWeapon(defaultWeapon);
            }
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
            GetComponent<Animator>().SetTrigger("attack"); //setting up attack animator
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetWeaponDamage()); 
            }
        }
        //Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange(); //we are not using navmeshagent.remainingdistance because it is not calculating real distance, - as remainingDistance is a calculation what what actually needs to be travelled to get to the location.
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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

        public object CaptureState()
        {
            //Debug.Log($"CaptureState - {currentWeapon.name}");
            return currentWeapon.name;
        }
        //names relative in Resources Folder
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            //Debug.Log($"RestoreState -- {weaponName}");
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            if (weapon == null)
            {
                //Debug.Log("The weapon was not found in Resources");
                return;
            }
            EquipWeapon(weapon);
        }
    }

}