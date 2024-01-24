using GameDevTV.Inventories;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class DialoguePickup : MonoBehaviour
{
    [SerializeField] InventoryItem item;
    [SerializeField] int number;
    Pickup pickup;

    private void Awake()
    {
        pickup = GetComponent<Pickup>();
        pickup.Setup(item, number);
    }

    public void PickupItem()
    {
        pickup.PickupItem();
    }
}
