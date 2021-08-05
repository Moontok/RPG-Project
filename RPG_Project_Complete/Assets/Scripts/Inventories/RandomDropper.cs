using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        // CONFIG DATA
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary = null;

        // CONSTANTS
        const int ATTEMPTS = 30;

        public void RandomDrop()
        {
            var baseStats = this.GetComponent<BaseStats>();
            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                
            DropItem(drop.item, drop.number);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            // We might need to try more than once to get on the NavMesh
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit = new NavMeshHit();
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }                
            }
            return this.transform.position;
        }
    }
}
