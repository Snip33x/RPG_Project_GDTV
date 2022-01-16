using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;

        void Update()
        {
            foreground.localScale = new Vector3(healthComponent.GetPercentage() / 100, 1f, 1f); //we can create a method in Health script GetFraction to avoid dividing by 100
        }
    }

}