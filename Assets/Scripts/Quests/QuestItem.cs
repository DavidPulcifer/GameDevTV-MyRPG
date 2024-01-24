using GameDevTV.Inventories;
using RPG.Dialogue;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = ("RPG/Quest Item"))]
public class QuestItem : InventoryItem
{
    [SerializeField] string questFlag;
    [SerializeField] bool removeFlag = false;
    PlayerConversant playerConversant;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerConversant = player.GetComponent<PlayerConversant>();
    }

    public void Use()
    {
        if(removeFlag)
        {
            playerConversant.RemoveDialogueFlag(questFlag);
        }
        else
        {
            playerConversant.AddDialogueFlag(questFlag);        
        }
    }
}
