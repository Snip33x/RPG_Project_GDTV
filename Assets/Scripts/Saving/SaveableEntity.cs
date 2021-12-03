using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways] //executes whenever we do anything in editing mode, for example, moving a mouse in edit window
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = ""; //unset ID at start
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();  //static lives through whole application

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())  //loping over components that has ISaveable, and asking them to capture states
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();  //capture string for example of "mover" and store it and it's state in dictionary //in compile time we would get Type Isaveable, but in runtime it changes, and we get desired type of component
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR //this code is removed from build 
        private void Update() {
            if (Application.IsPlaying(gameObject)) return; //check if we are playing
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //we don't want to give an ID to prefab // prefab does't have path

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();  //128 bit codes generated randomly, globally unique idntifier (guid) / universally unique identifier - UUiD there is a chance that 2 same ID's will be generated but it's super small chances
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)  //when duplicating, for example enemy, the UUID was't generating a new ID, this method makes our system to genereate new ID when we copy enemy
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)  // when changing scenes, enemies are no longer changing UUID's
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}