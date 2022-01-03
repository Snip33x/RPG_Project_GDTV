
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
           
            SetCursor(CursorType.None);
            print("Nothing to do");
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100); //casting ray line
        }

        private bool InteractWithUI()
        {  
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) // making our mouse skip raycasting dead bodies
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetCursor(CursorType.Combat);
                return true; //interaction with combat should start when we hover over enemy (attack cursor)
            }
            return false; // we didn't find any enemies to interact with 
        }


        private bool InteractWithMovement()
        {

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);  //raycast is flying all the time, and below we can imlemepnt hovering over with mouse
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f); //1f - move at max speed
                }
                SetCursor(CursorType.Movement);
                return true; //interaction with movement should start when we hover over enemy (move cursor)
            }            
            return false; // we didn't find any place to move to 
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0]; //use 1st cursor from array if something goes wrong
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
