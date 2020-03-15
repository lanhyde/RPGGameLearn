using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,  99), SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool shouldUseModifiers = true;
        public event Action onLevelUp;
        private Experience experience;
        private LazyValue<int> currentLevel;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start() 
        {
            currentLevel.ForceInit();

        }

        void OnEnable()
        {
            if (experience)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        void OnDisable()
        {
            if (experience)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public int GetLevel()
        {
            return currentLevel;
        }
        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(!experience) return startingLevel;
            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for(int level = 1; level <= penultimateLevel; ++level)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(currentXP < xpToLevelUp)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }

}