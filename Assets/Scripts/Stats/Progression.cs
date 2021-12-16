using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)] //showing when we right click mouse in editor-> create
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health;
        }
    }

}