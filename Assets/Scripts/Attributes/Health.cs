using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //[SerializeField] float regenerationPercentage = 70;  //regen 70% of hp, instead of 100% when lvlup, rest of code in RegenerateHP()

        [SerializeField] TakeDamageEvent takeDamage;  //UnityEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> //serialized field doesn't allow generics so we had to do this walk-around
        {

        }

        LazyValue<float> healthPoints;

        bool isDead = false;

        //BaseStats basestats;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
            /*basestats = GetComponent<BaseStats>();*/
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit(); // if we haven't at this point accesed Health, and caused it to Initialize, we will do it now in Start
            //if (healthPoints < 0) // if we killed guard and saved, and then played game again, saved and played again the guard was alive, because his health was set to 40, and the reason was that restore state wass called before setting health with our system
            //{
            //    healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health); //Data Race
            //}                        
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHP;
            /*if (basestats != null) //we are calling event and if there would be no subscribers , the error would be thrown, so we protect it by checking null
            {
                basestats.onLevelUp += RegenerateHP; //+= is subsctibing to delegate, event is Experience prevent it from overwriting
            }*/
        }

        private void OnDisable() //good practices
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHP;
            /*if (basestats != null) //we are calling event and if there would be no subscribers , the error would be thrown, so we protect it by checking null
            {
                basestats.onLevelUp -= RegenerateHP;
            }*/
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)  //instigator - the one that started the fight will be granted with xp
        {
            print(gameObject.name + " took damage: " + damage);

            //health -= damage; // my example to make health not go beyond 0
            //if (health < 0)
            //    health = 0;
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            print(healthPoints.value); //cos
            if(healthPoints.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator); // we need to tell who will gain the experience - the instigator
            }
            else
            {
                takeDamage.Invoke(damage); //unity event
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHeatlhPoints()//this is forwarding method, we use this because wo would need to make a dependency in HealthDisplay to BaseStats, and its logical that we get MaxHP from Health Component
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHP()
        {
            // Regenerate to over 70% of max hp of new LVL, but if we have more like 90 % dont do anything
            //float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            //healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        #region Saveable Interface
        public object CaptureState()
        {
            return healthPoints.value;
        }
        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Die();
            }
        }
        #endregion
    }
}
