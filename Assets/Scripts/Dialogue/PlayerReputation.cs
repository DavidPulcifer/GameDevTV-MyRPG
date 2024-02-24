using GameDevTV.Saving;
using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerReputation : MonoBehaviour, IPredicateEvaluator, ISaveable, IJsonSaveable
    {
        Dictionary<string, int> factionReputation;
        TraitStore traitStore;

        private void Awake()
        {
            factionReputation = new Dictionary<string, int>();
            traitStore = GetComponent<TraitStore>();
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

        public int GetReputation(string factionName, bool rawScore)
        {
            if (factionReputation.TryGetValue(factionName, out int rep))
            {
                if(rawScore)
                {
                    return rep;

                } 
                else
                {
                    return rep + (int)Mathf.Floor(traitStore.GetPoints(Trait.Charisma) / 2);
                }
            }
            else
            {
                if (rawScore)
                {
                    return 0;

                }
                else
                {
                    return (int)Mathf.Floor(traitStore.GetPoints(Trait.Charisma) / 2);
                }
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasGoodRep":
                    return GetReputation(parameters[0], false) > 0;
                case "HasGoodRepRaw":
                    return GetReputation(parameters[0], true) > 0;
                case "HasNeutralRep":
                    return GetReputation(parameters[0], false) == 0;
                case "HasNeutralRepRaw":
                    return GetReputation(parameters[0], true) == 0;
                case "HasBadRep":
                    return GetReputation(parameters[0], false) < 0;
                case "HasBadRepRaw":
                    return GetReputation(parameters[0], true) < 0;
                case "HasInexcusableRep":                    
                    return GetReputation(parameters[0], false) < -1;
            }

            return null;
        }

        [System.Serializable]
        struct ReputationRecord
        {
            public string faction;
            public int reputation;
        }

        public object CaptureState()
        {            
            return factionReputation;
        }

        public void RestoreState(object state)
        {
            factionReputation = (Dictionary<string, int>)state;            
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(factionReputation);
        }

        public void RestoreFromJToken(JToken state)
        {
            factionReputation = state.ToObject<Dictionary<string, int>>();
        }
    }

}

