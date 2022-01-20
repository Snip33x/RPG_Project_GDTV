using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {

        void LateUpdate()
        {
            //transform.LookAt(FindObjectOfType<Camera>().transform); //heavy
            transform.forward = Camera.main.transform.forward; // make text face same direction as Camera
        }
    }

}