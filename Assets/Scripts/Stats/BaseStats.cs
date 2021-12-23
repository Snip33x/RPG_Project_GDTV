using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null) //we are calling event and if there would be no subscribers , the error would be thrown, so we protect it by checking null
            {
                experience.onExperienceGained += UpdateLevel; //+= is subsctibing to delegate, event is Experience prevent it from overwriting
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                onLevelUp();
                LevelUpEffect();
            }

        }

        private void LevelUpEffect()
        {
            if (levelUpEffect != null)
            {
                Instantiate(levelUpEffect, transform);
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100); //GetAdditiveModifier(stat) - stat argument is a character current level damage // multiply by 1.1 dmg for example
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel <1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in this.GetComponents<IModifierProvider>()) //In short, the interface not only says "I do solomly swear to implement these methods in my script’, it also allows you go gather all scripts that implement these methods into a collection. Because of Interfaces, BaseStats doesn’t have to know anything about Fighter. No need to add a using RPG.Combat. It’s not required in the least. It just needs to know that the object it has retrieved is an IModifierProvider, nothing more.  //Bear in mind that this also means that BaseStats can ONLY see GetAdditiveModifiers() and GetPercentageModifiers(). It can’t access anything else that is not in the interface. It can’t look at the transform, or tell what the current target is, or anything else. That’s by design, all BaseStats needs to know is “Hey, all you IModifierProviders out there, would you please be so kind as to tally up the AdditiveModifiers for me?”
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat)) //this is a call to a Fighter for example because it has current weapon dmg in float to return and it will sum all the modifiers to return full dmg
                {
                    total += modifier;
                }
            }
            return total;
            
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in this.GetComponents<IModifierProvider>()) 
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;

        }


        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel; //enemy don't give xp, just return early

            float currentXp = experience.GetExperiencePoint();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXp)
                {
                    return level; // if we have less xp then required to next lvl, then we are that lvl
                }

            }

            return penultimateLevel + 1; //experience was higher then lvl, so we must be above
        }


    }
}
