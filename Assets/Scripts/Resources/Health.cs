using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100;
        bool isDead = false;

        void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if(healthPoints == 0)
            {
                Die();
            }
        }

        public float GetPercentage()
        {
            return 100f * (healthPoints / GetComponent<BaseStats>().GetHealth());
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
           healthPoints = (float)state;
           if(healthPoints == 0)
           {
               Die();
           }
        }

    }
}