using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Agent : MonoBehaviour, IComparable<Agent>
{
    #region Variable
    [Header ("Stats")]
    public float fitness;
    public float distanceTraveled;

    [Header ("Neural systeme")]
    public NeuralNetwork net;
    [Space]
    public float[] inputs;

    [Header ("Script")]

    public CarController carController;

    [Header ("GFX")]
    public Material firstMat;
    public Material defaultMat;
    public Material mutateMat;

    public MeshRenderer mapFeedBackRenderer;

    [Header ("Nextpoint")]
    public Transform nextCheckpoint;
    public float nextCheckpointDist;

    [Header ("Physics")]
    public Rigidbody rb;
    public Transform rayStart;
    public LayerMask layerMask;
    public LayerMask layerObstacle;
    public LayerMask layerJumpDetection;
    public float rayRange;
    public Vector3 offsetJumpRay;
    RaycastHit hit;
    #endregion

    private void Awake()
    {
        ResetAgent();
    }

    private void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();
        FitnessUpdate();
    }

    void InputUpdate()
    {
        inputs[0] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward, 3.5f, layerMask);
        inputs[1] = RaySensor(transform.position + Vector3.up * 0.2f, transform.right, 3.5f, layerMask);
        inputs[2] = RaySensor(transform.position + Vector3.up * 0.2f, -transform.right, 3.5f, layerMask);
        inputs[3] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward + transform.right, 2f, layerMask);
        inputs[4] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward - transform.right, 2f, layerMask);

        inputs[5] = (float)Math.Tanh(rb.velocity.magnitude / 0.05f);
        inputs[6] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);
        inputs[7] = 1f;

        inputs[8] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward, 3.5f, layerObstacle);

        float hight = RaySensor(rayStart.position, Vector3.down, 1.9f , layerJumpDetection);

        if (hight > 0)
        {
            hight = 1;
        }
        else
        {
            hight = 0f;
        }

        inputs[9] = hight;
    }
    void OutputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontaleInput = net.neurons[net.layers.Length - 1][0];
        carController.verticaleInput = net.neurons[net.layers.Length - 1][1];

        carController.hight = net.neurons[net.layers.Length - 1][2];
    }

    float tempDistance;
    void FitnessUpdate()
    {
        tempDistance = distanceTraveled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDistance)
        {
            fitness = tempDistance; 
        }
    }


    public void ResetAgent()
    {
        fitness = 0f;
        distanceTraveled = 0f;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        inputs = new float[net.layers[0]];

        carController.ResetAxes();

        nextCheckpoint = CheckPointManager.instance.firstCheckPoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }
    public void CheckpointReached(Transform checkpoint)
    {
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = checkpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void SetFirstColor()
    {
        GetComponent<MeshRenderer>().material = firstMat;
        mapFeedBackRenderer.material = firstMat;
    }

    public void SetDefaultColor()
    {
        GetComponent<MeshRenderer>().material = defaultMat;
        mapFeedBackRenderer.material = defaultMat;
    }

    public void SetMutateColor()
    {
        GetComponent<MeshRenderer>().material = mutateMat;
        mapFeedBackRenderer.material = mutateMat;
    }

    public int CompareTo(Agent other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }
        if (fitness > other.fitness)
        {
            return -1;
        }
        return 0;
    }

    float RaySensor(Vector3 pos, Vector3 direction, float lenght, LayerMask _layerMask)
    {
        if (Physics.Raycast(pos, direction, out hit, lenght*rayRange, _layerMask))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.Lerp(Color.red, Color.green, (rayRange * lenght - hit.distance) / (rayRange * lenght)));

            return (rayRange * lenght - hit.distance) / (rayRange*lenght);
        }
        else
        {
            Debug.DrawRay(pos, direction * rayRange * lenght, Color.red);
        }

        return 0f;
    }
}
