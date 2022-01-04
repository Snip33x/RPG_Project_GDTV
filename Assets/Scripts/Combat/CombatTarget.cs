using RPG.Attributes;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) // making our mouse skip raycasting dead bodies
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true; //interaction with combat should start when we hover over enemy (attack cursor)
        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}