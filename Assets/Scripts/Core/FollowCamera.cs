using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;


        void LateUpdate() // The camera will update after the player has moved, otherway we will get some skipping frames bugs
        {
            transform.position = target.position;
        }
    }
}
