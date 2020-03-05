using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool isTriggered = false;

        private void OnTriggerEnter(Collider other) 
        {
            if(!isTriggered && other.tag == "Player")
            {
                isTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
        
        public object CaptureState()
        {
            return isTriggered;
        }

        public void RestoreState(object state)
        {
            isTriggered = (bool)state;
        }
    }

}