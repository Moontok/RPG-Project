using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target = null;

    void LateUpdate()
    {
        this.transform.position = target.position;
    }
}
