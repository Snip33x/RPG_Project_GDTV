using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }


        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            //health -= damage; // my example to make health not go beyond 0
            //if (health < 0)
            //    health = 0;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints); //cos
            if(healthPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
