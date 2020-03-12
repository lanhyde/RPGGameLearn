using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        float experiencePoints = 0;

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public float GetPoints()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

    }
}