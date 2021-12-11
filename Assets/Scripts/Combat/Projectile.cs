using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSped = 1f;
        [SerializeField] Transform target = null;

        void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSped);
        }

        private Vector3 GetAimLocation() //make our arrow shot at the center mass of the target
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.position;
            }
            return target.position + Vector3.up * targetCapsule.height / 2;
        }
    }

}