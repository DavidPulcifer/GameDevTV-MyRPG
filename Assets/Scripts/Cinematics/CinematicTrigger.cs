using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable, IJsonSaveable
    {
        bool hasPlayed = false;
       
        private void OnTriggerEnter(Collider other)
        {
            if(!hasPlayed && other.tag == "Player")
            {
                hasPlayed = true;
                GetComponent<PlayableDirector>().Play();                
            }            
        }

        public object CaptureState()
        {
            return hasPlayed;
        }

        public void RestoreState(object state)
        {
            hasPlayed = (bool)state;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(hasPlayed);
        }

        public void RestoreFromJToken(JToken state)
        {
            hasPlayed = state.ToObject<bool>();
        }
    }
}

