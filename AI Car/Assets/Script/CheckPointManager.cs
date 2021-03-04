﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    public Transform firstCheckPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    [ContextMenu ("Set CheckPoint")]
    public void Init()
    {
        firstCheckPoint = transform.GetChild(0);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetComponent<CheckPoint>().nextCheckpoint = transform.GetChild(i + 1);
        }

        transform.GetChild(transform.childCount - 1).GetComponent<CheckPoint>().nextCheckpoint = transform.GetChild(0);
    }
}
