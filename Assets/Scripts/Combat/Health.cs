using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

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
        }
    }
}
