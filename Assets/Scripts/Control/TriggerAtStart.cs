using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class TriggerAtStart : MonoBehaviour
    {
        [SerializeField] UnityEvent onStart;

        // Start is called before the first frame update
        void Start()
        {
            onStart.Invoke();
        }        
    }
}
