using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 cameraLocalPosition;
    public Vector3 localTargetLookAtPosition;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.1f;

    Vector3 wantedPos;

    private void Update()
    {
        
        wantedPos = target.TransformPoint(cameraLocalPosition);
        wantedPos.y = cameraLocalPosition.y + target.transform.position.y;

        transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);
        Quaternion look = Quaternion.LookRotation(target.TransformPoint(localTargetLookAtPosition) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, look, lookLerpSpeed);
        
    }
    


}
