using GameDevTV.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerReputation : MonoBehaviour, IPredicateEvaluator
    {
        Dictionary<string, int> factionReputation;

        private void Awake()
        {
            factionReputation = new Dictionary<string, int>();
        }

        public void ModifyReputation(string factionName, int points)
        {
            // If the faction does not exist, add it with a default reputation of 0
            if (!factionReputation.ContainsKey(factionName))
            {
                factionReputation.Add(factionName, 0);
            }

            // Modify the reputation of the faction
            factionReputation[factionName] += points;
        }

        public void ImproveReputation(string factionName)
        {
            ModifyReputation(factionName, 1);
        }

        public void HarmReputation(string factionName)
        {
            ModifyReputation(factionName, -1);
        }

        public void DestroyReputation(string factionName)
        {
            ModifyReputation(factionName, -100);
        }

        public int GetReputation(string factionName)
        {
            if (factionReputation.TryGetValue(factionName, out int rep))
            {
                return rep;
            }
            else
            {                
                return 0;
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasGoodRep":
                    return GetReputation(parameters[0]) > 0;
                case "HasBadRep":
                    return GetReputation(parameters[0]) < 0;
                case "HasInexcusableRep":
                    Debug.Log(GetReputation(parameters[0]) < -1);
                    return GetReputation(parameters[0]) < -1;
            }

            return null;
        }
    }

}

