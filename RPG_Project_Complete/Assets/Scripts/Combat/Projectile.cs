using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float bonusDamage = 0f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = .2f;
        [SerializeField] UnityEvent onHit = null;

        Health target = null;
        Vector3 targetPoint = default;
        GameObject instigator = null;
        float damage = 0f;

        private void Start() 
        {
            this.transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target != null && isHoming && !target.IsDead()) 
            { 
                this.transform.LookAt(GetAimLocation()); 
            }
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage + bonusDamage;
            this.instigator = instigator;

            Destroy(this.gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }

            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) 
            { 
                return target.transform.position; 
            }
            
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();

            if (target != null && health != target) { return; }
            if (health == null || health.IsDead()) { return; }
            if (other.gameObject == instigator) { return; }
            health.TakeDamage(instigator, damage);
            ProjectileImpact();
        }

        private void ProjectileImpact()
        {
            speed = 0f;

            onHit.Invoke();

            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), this.transform.rotation);

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(this.gameObject, lifeAfterImpact);
        }
    }
}
