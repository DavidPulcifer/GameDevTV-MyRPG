using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class TriggerOnGameObjectCollide : MonoBehaviour
    {
        [SerializeField] GameObject targetObject;
        [SerializeField] UnityEvent onCollide;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == targetObject)
            {
                onCollide.Invoke();
            }
        }
    }
}