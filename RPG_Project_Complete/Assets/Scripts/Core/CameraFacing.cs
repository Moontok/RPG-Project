using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}
