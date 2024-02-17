using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour, IPredicateEvaluator
    {
        [SerializeField] string playerName;
        [SerializeField] List<string> dialogueFlags = new List<string>();

        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;
                
        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            if (IsActive()) Quit();
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Quit()
        {
            currentConversant.ResetPosition();
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            currentConversant = null;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if(currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        public AIConversant GetCurrentConversant()
        {
            return currentConversant;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {            
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public void AddDialogueFlag(string flag)
        {
            if (!dialogueFlags.Contains(flag))
            {
                dialogueFlags.Add(flag);
            }
        }

        public void RemoveDialogueFlag(string flag)
        {
            if (dialogueFlags.Contains(flag))
            {
                dialogueFlags.Remove(flag);
            }
        }

        public bool HasDialogueFlag(string flag)
        {
            return dialogueFlags.Contains(flag);
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        void TriggerEnterAction()
        {
            if(currentNode != null && currentNode.GetOnEnterAction().Length != 0)
            {
                foreach (string action in currentNode.GetOnEnterAction())
                {
                    TriggerAction(action);
                }
            }
        }

        void TriggerExitAction()
        {
            if (currentNode != null && currentNode.GetOnExitAction().Length != 0)
            {
                foreach (string action in currentNode.GetOnExitAction())
                {
                    TriggerAction(action);
                }
            }
        }

        void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasDialogueFlag":
                    foreach (string parameter in parameters)
                    {
                        if (dialogueFlags.Contains(parameter))
                        {
                            return true;
                        }
                    }
                    return false;

            }
            return null;
        }
    }

}