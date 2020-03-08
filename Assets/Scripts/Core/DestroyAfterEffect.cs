using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private ParticleSystem particleSystem;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();    
        }
        void Update()
        {
            if(!particleSystem.IsAlive()) 
            {
                Destroy(gameObject);
            }
        }
    }

}