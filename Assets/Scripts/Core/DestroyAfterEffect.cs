using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDesstroy = null;
        private ParticleSystem particleSystem;

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();    
        }
        void Update()
        {
            if(!particleSystem.IsAlive()) 
            {
                if (targetToDesstroy)
                {
                    Destroy(targetToDesstroy);
                }
                Destroy(gameObject);
            }
        }
    }

}