using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{

    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        // Start is called before the first frame update
        void Start()
        {
            Spawn(11);
        }

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);  //we are using Generic type of Instantiate because we know the type of our prefab is DamageText?
        }
    }

}