using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
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

    }
}

