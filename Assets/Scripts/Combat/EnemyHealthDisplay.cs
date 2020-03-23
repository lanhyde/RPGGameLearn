using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay: MonoBehaviour
    {
        Health health;
        Fighter playerFighter;
        Text healthText;
        private void Awake()
        {
            healthText = GetComponent<Text>();
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() 
        {
            if(!playerFighter.GetTarget())
            {
                healthText.text = "N/A";
            }
            else 
            {
                healthText.text = string.Format("{0:0}/{1:0}", playerFighter.GetTarget().GetHealthPoints(), playerFighter.GetTarget().GetMaxHealthPoints());
            }
        }
    }
}