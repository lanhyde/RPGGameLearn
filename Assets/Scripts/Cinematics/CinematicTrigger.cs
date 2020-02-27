using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
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
    }

}