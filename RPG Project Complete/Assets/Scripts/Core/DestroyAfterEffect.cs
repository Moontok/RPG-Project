using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        void Update()
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
                
            }
        }
    }
}
