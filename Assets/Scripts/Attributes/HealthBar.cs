using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            if (Mathf.Approximately(healthComponent.GetPercentage() ,0) //float is imprecise , don't compare float to 0, use this method
            || Mathf.Approximately(healthComponent.GetPercentage(), 100)) 
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetPercentage() / 100, 1f, 1f); //we can create a method in Health script GetFraction to avoid dividing by 100

        }
    }

}