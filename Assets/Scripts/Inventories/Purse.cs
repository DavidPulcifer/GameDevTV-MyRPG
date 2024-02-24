using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace RPG.Inventories { 
    public class Purse : MonoBehaviour, ISaveable, IItemStore, IPredicateEvaluator, IJsonSaveable
    {
        [SerializeField] float startingBalance = 400f;

        float balance = 0;

        public event Action onChange;

        private void Awake()
        {
            balance = startingBalance;            
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            if(onChange != null)
            {
                onChange();
            }
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
        }

        public int AddItems(InventoryItem item, int number)
        {
            if(item is CurrencyItem)
            {
                UpdateBalance(item.GetPrice() * number);
                return number;
            }
            return 0;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate == "MinimumBalance")
            {
                return GetBalance() >= Int32.Parse(parameters[0]);
            }
            return null;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(balance);
        }

        public void RestoreFromJToken(JToken state)
        {
            balance = state.ToObject<float>();
        }

    }
}
