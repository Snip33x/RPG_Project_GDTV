using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public void TakeDamage(float damage)
        {
            //health -= damage; // my example to make health not go beyond 0
            //if (health < 0)
            //    health = 0;
            health = Mathf.Max(health - damage, 0);
            print(health); //cos
        }
    }
}
