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
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            if(lookupTable[characterClass][stat].Length > level)
            {
                return lookupTable[characterClass][stat][level - 1];
            }
            return 0;
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            return lookupTable[characterClass][stat].Length;
        }
        private void BuildLookup()
        {
            if(lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach(var progressionClass in characterClasses)
            {
                var stateLookupTable = new Dictionary<Stat, float[]>();
                foreach(var stat in progressionClass.stats)
                {
                    stateLookupTable.Add(stat.stat, stat.levels);
                }
                lookupTable[progressionClass.characterClass] = stateLookupTable;
            }
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
