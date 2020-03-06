﻿using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;


        void Update()
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
                if(targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}