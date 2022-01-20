using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSped = 1f;
        [SerializeField] bool isHoming = true; // chasing target
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float timeToDestroy = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onProjectileHit;


        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()  
        {
            transform.LookAt(GetAimLocation());  //everytime projectile is Instantiated we set target location
            Destroy(gameObject, timeToDestroy);
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

        public void SetTarget(Health target, GameObject instigator, float damage) //we are calculating damage done by projectile at the shoot moment
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
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
            
            target.TakeDamage(instigator, damage);

            projectileSped = 0; //prevent arrow from going further the target - it's happening because fireball is part destroyed within below code ,and this line is alsom making projectile trail to partly vanish

            onProjectileHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit) //first destroy fireball's head, then rest after 2 secs
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);

        }
    }

}