using System.Collections;
using UnityEngine;

namespace RPG.Environment
{
    public class FlameFlicker : MonoBehaviour
    {
        [SerializeField] float flameShift = .01f;
        [SerializeField] float MaxReduction = 0.5f;
        [SerializeField] float MaxIncrease = 0.5f;
        [SerializeField] float RateDamping = 0.05f;
        [SerializeField] float Strength = 300;
        [SerializeField] bool StopFlickering = false;
    
        Light lightSource = null;
        Vector3 startingPosition = new Vector3();
        float baseIntensity = 0f;
        bool flickering = true;
    
        public void Reset()
        {
            MaxReduction = 0.2f;
            MaxIncrease = 0.2f;
            RateDamping = 0.1f;
            Strength = 300;
        }
    
        public void Start()
        {
            lightSource = this.GetComponent<Light>();
            startingPosition = this.transform.localPosition;
            baseIntensity = lightSource.intensity;
            StartCoroutine(DoFlicker());
        }
    
        void Update()
        {
            if (!StopFlickering && !flickering)
            {
                StartCoroutine(DoFlicker());
            }
        }
    
        private IEnumerator DoFlicker()
        {
            flickering = true;
            while (!StopFlickering)
            {
                lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(baseIntensity - MaxReduction, baseIntensity + MaxIncrease), Strength * Time.deltaTime);
                float shiftX = startingPosition.z + Random.Range(-flameShift, flameShift);
                float shiftY = startingPosition.y + Random.Range(-flameShift, flameShift);
                float shiftZ = startingPosition.z + Random.Range(-flameShift, flameShift);
                this.transform.localPosition = new Vector3(shiftX, shiftY, shiftZ);
                yield return new WaitForSeconds(RateDamping);
            }
            flickering = false;
        }
    }
}
