﻿using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        private Health target = null;
        [SerializeField]
        private float speed = 1.0f;
        [SerializeField]
        private bool allowHoming = true;
        [SerializeField]
        GameObject hitEffect = null;
        [SerializeField]
        float maxLifetime = 10f;
        [SerializeField]
        GameObject[] destroyOnHit = null;
        [SerializeField]
        float lifeAterImpact = 2f;
        [SerializeField]
        private UnityEvent onProjectileHit;
        GameObject instigator = null;
        float damage = 0;

        private void Start () {
            transform.LookAt (GetAimLocation ());
        }
        // Update is called once per frame
        void Update () {
            if (!target) return;
            if (allowHoming && !target.IsDead ()) {
                transform.LookAt (GetAimLocation ());
            }
            transform.Translate (Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget (Health target, GameObject instigator, float damage) {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy (gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation () {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider> ();
            if (!targetCapsule) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter (Collider other) {
            if (other.GetComponent<Health> () != target) return;
            if (target.IsDead ()) return;

            target.TakeDamage (instigator, damage);

            speed = 0;
            onProjectileHit.Invoke ();
            if (hitEffect) {
                Instantiate (hitEffect, GetAimLocation (), transform.rotation);
            }
            foreach (GameObject toDestroy in destroyOnHit) {
                Destroy (toDestroy);
            }
            Destroy (gameObject, lifeAterImpact);
        }
    }
}