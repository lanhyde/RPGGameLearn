using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regeneratePercentage = 70;
        private LazyValue<float> healthPoints;
        bool isDead = false;
        private BaseStats stats;

        void Awake()
        {
            stats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(() => 
                GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        void OnEnable()
        {
           stats.onLevelUp += RegenerateHealth;
        }

        void OnDisable()
        {
            stats.onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);
            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if(!experience) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return 100f * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if(isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
           healthPoints.value = (float)state;
           if(Math.Abs(healthPoints) < 0.0001f)
           {
               Die();
           }
        }

    }
}