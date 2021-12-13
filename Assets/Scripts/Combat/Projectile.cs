using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSped = 1f;
        [SerializeField] bool isHoming = true; // chasing target

        Health target = null;
        float damage = 0;


        private void Start()  
        {
            transform.LookAt(GetAimLocation());  //everytime projectile is Instantiated we set target location
        }

        void Update()
        {
            if (target == null) return;


            if (isHoming && !target.IsDead()) //avoid arrow sticking in air at target location when target is dead
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSped);

        }

        public void SetTarget(Health target, float damage) //we are calculating damage done by projectile at the shoot moment
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation() //make our arrow shot at the center mass of the target
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return; //if enemy is dead don't try to give any damage and don't destroy object
            target.TakeDamage(damage);
            Destroy(gameObject);

        }
    }

}