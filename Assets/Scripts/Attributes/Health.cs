using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //[SerializeField] float regenerationPercentage = 70;  //regen 70% of hp, instead of 100% when lvlup, rest of code in RegenerateHP()

        float healthPoints = -1f;

        bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0) // if we killed guard and saved, and then played game again, saved and played again the guard was alive, because his health was set to 40, and the reason was that restore state wass called before setting health with our system
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            BaseStats basestats = GetComponent<BaseStats>();
            if (basestats != null) //we are calling event and if there would be no subscribers , the error would be thrown, so we protect it by checking null
            {
                basestats.onLevelUp += RegenerateHP; //+= is subsctibing to delegate, event is Experience prevent it from overwriting
            }
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
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints); //cos
            if(healthPoints <= 0)
            {
                Die();
                AwardExperience(instigator); // we need to tell who will gain the experience - the instigator
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHeatlhPoints()//this is forwarding method, we use this because wo would need to make a dependency in HealthDisplay to BaseStats, and its logical that we get MaxHP from Health Component
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        #region Saveable Interface
        public object CaptureState()
        {
            return healthPoints;
        }
        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                Die();
            }
        }
        #endregion
    }
}
