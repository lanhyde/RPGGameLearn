using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (var progressionClass in this.characterClasses)
            {
                if(progressionClass.characterClass != characterClass) continue;
                foreach(ProgressionStat progressionStat in progressionClass.stats) 
                {
                    if(progressionStat.stat != stat) continue;
                    if(progressionStat.levels.Length < level) continue;
                    return progressionStat.levels[level - 1];
                }
            }

            return 0;
        }
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;

            public ProgressionStat[] stats;
        }
        [System.Serializable]
        class ProgressionStat 
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
