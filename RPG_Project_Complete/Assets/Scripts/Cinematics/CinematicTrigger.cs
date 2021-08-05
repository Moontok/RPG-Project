using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other) 
        {
            if (!alreadyTriggered && other.tag == "Player")
            {
                alreadyTriggered = true;
                this.GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
