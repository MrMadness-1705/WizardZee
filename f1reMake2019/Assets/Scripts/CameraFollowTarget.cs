using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Color backgroundColor;

    [Range(1, 10)]
    public float smoothFactor = 2f;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.fixedDeltaTime);
            transform.position = smoothPosition;
            //GetComponent<Camera>().backgroundColor = backgroundColor;
        }
    }
}
