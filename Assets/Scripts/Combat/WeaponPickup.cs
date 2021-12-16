using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float timeToRespawnAnItem = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(timeToRespawnAnItem)); //we are not disabling whole gameObject because the Coroutine would fail to run
            }
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


    }
}

