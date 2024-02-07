using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class TriggerOnCollide : MonoBehaviour
    {
        [SerializeField] UnityEvent onCollide;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                onCollide.Invoke();
            }
        }
    }
}
