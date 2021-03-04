﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform nextCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Agent>())
        {
            if (other.transform.parent.GetComponent<Agent>().nextCheckpoint == transform)
            {
                other.transform.parent.GetComponent<Agent>().CheckpointReached(nextCheckpoint);
            }
        }
    }
}
