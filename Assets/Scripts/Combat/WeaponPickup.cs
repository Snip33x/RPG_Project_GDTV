using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float timeToRespawnAnItem = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subjcet)
        {
            if (weapon != null)
            {
                subjcet.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (healthToRestore > 0)
            {
                subjcet.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(timeToRespawnAnItem)); //we are not disabling whole gameObject because the Coroutine would fail to run
        }

        private IEnumerator HideForSeconds(float seconds) 
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        //!!! Disabling and reenabling childs in Parent 
        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform) //transform is refering to this object transform
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true; //howering over pickup will handle this raycast, we won't be abe to walk 
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}

