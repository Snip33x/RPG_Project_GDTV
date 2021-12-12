using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)] //showing when we right click mouse in editor-> create
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if (equippedPrefab != null) // if you don't have Prefab equipped then don't try to Instatiate that equipped Prefab
            {
                Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform) // we have a bool used in editor to make our weapon right or left handed
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHandTransform;
            else handTransform = leftHandTransform;
            return handTransform;
        }

        public bool HasProjectile() // do we have projectile equipped
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
    }
}