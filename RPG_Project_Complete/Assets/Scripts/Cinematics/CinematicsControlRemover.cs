using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        GameObject player = null;

        private void Awake() 
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() 
        {            
            this.GetComponent<PlayableDirector>().played += DisableControl;
            this.GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() 
        {
            this.GetComponent<PlayableDirector>().played -= DisableControl;
            this.GetComponent<PlayableDirector>().stopped -= EnableControl;            
        }

        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
