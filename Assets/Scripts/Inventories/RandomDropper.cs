using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        //CONFIG DATA
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] InventoryItem[] dropLibrary;
        [SerializeField] int numberOFDrops = 2;

        //CONSTANTS
        const int ATTEMPTS = 30;

        public void RandomDrop()
        {
            for (int i = 0; i < numberOFDrops; i++)
            {
                var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
                DropItem(item, 1);
            }            
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }            
            return transform.position;
        }
    }
}
