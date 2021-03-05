using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region variable
    [Header ("Car stats")]
    public float maxSteerAngle = 42f;
    public float motoForce = 1000f;

    [Space]

    [Header("Wheel Setup")]

    public WheelCollider wheelFrontLeftCollider;
    public WheelCollider wheelFrontRightCollider;
    public WheelCollider wheelRearLeftCollider;
    public WheelCollider wheelRearRightCollider;

    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform wheelRearLeft;
    public Transform wheelRearRight;

    [Space]

    [Header ("Physics")]
    public Rigidbody carRb;
    public Transform centerOfMass;

    [Header ("Axes")]
    public float horizontaleInput;
    public float verticaleInput;
    float steeringAngle;

    [Header ("Jump")]
    public float hight;
    public float jumpForce = 1000f;
    public bool canJump = true;
    public float disFromObj;
    #endregion

    private void Start()
    {
        canJump = true;
        carRb.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
        Jump();
    }

    void Steer()
    {
        steeringAngle = horizontaleInput * maxSteerAngle;

        wheelFrontLeftCollider.steerAngle = steeringAngle;
        wheelFrontRightCollider.steerAngle = steeringAngle;
    }
    void Accelerate()
    {
        wheelFrontLeftCollider.motorTorque = verticaleInput * motoForce;
        wheelFrontRightCollider.motorTorque = verticaleInput * motoForce;

        wheelRearLeftCollider.motorTorque = verticaleInput * motoForce;
        wheelRearRightCollider.motorTorque = verticaleInput * motoForce;
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPos(wheelFrontLeftCollider, wheelFrontLeft);
        UpdateWheelPos(wheelFrontRightCollider, wheelFrontRight);
        UpdateWheelPos(wheelRearLeftCollider, wheelRearLeft);
        UpdateWheelPos(wheelRearRightCollider, wheelRearRight);
    }

    void Jump()

    {
        if (canJump)
        {
            if (hight > 0.5f)
            {
                canJump = false;
                carRb.AddForce(Vector3.up * jumpForce);
                carRb.constraints = RigidbodyConstraints.FreezeRotationY;
            }
        }
        
    }

    Vector3 pos;
    Quaternion quat;

    void UpdateWheelPos(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }

    public void ResetAxes()
    {
        horizontaleInput = 0;
        verticaleInput = 0;
    }

    bool canCoro = true;
    IEnumerator ResetCanJump()
    {
        if (!canJump)
        {
            if (canCoro)
            {
                canCoro = false;
                yield return new WaitForSeconds(1f);
                canJump = true;
                canCoro = true;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            canJump = true;
            carRb.constraints = RigidbodyConstraints.None;
        }
    }
}
