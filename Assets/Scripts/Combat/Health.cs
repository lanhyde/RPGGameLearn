using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100;
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(0, health - damage);
            print(health);
        }
    }
}