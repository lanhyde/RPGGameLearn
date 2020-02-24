using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        float chaseDistance = 5f;
        private Fighter fighter;
        private GameObject player;

        private void Start()
        {
            fighter = GetComponent<Fighter>();   
            player = GameObject.FindWithTag("Player"); 
        }
        void Update()
        {
            if(InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
               fighter.Attack(player);
            }
            else 
            {
                fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }
    }

}
