using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float bonusDamage = 0f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;

        float damage = 0f;
        Health target = null;

        private void Start() 
        {
            this.transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead()) this.transform.LookAt(GetAimLocation());
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage + bonusDamage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(damage);

            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), this.transform.rotation);

            Destroy(this.gameObject);
        }
    }
}
