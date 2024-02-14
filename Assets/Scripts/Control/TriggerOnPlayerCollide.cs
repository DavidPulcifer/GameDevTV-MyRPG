using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class TriggerOnPlayerCollide : MonoBehaviour
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
