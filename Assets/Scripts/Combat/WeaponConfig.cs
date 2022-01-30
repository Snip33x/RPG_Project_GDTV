using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)] //showing when we right click mouse in editor-> create
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float PercentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return PercentageBonus;
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null) // if you don't have Prefab equipped then don't try to Instatiate that equipped Prefab
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName; //naming our weapon gameObject
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {                
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null) //we had bug - when we don't have animatorOverrideController attached to weapon, and we were using another weapon before it, the last weapon animator would work for that weapon, now the default animation is punch
            {                
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName); //we are looking if there is anything in a rightHand
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;  //we dont have any weapon so do nothing

            oldWeapon.name = "DESTROYING"; //there may be a frame where a new and old weapon are called "Weapon", so we change name of old weapon to protect from that bug
            Destroy(oldWeapon.gameObject);
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);

            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
    }
}