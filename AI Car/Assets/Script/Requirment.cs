using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Requirment : MonoBehaviour, IComparable<Requirment>
{
    public static Requirment instance;

    public LayerMask layerMask;
    public float value;

    public List<int> listInt = new List<int>();

    public int[] arrayInt;

    public int[][] jaggedArray2dOfInt;

    public int[][][] jaggedArray3dOfInt;

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

    public int CompareTo(Requirment other)
    {
        if (value < other.value)
        {
            return 1;
        }

        if (value > other.value)
        {
            return -1;
        }

        return 0;
    }

    private void Start()
    {
        //List();
        //ArrayTest();
        //Jagged2dArray();
        //Jagged3dArray();
        RaycastTest();
    }


    void List()
    {
        listInt.Add(item: 123);
        int myInt = 321;
        listInt.Add(myInt);

        listInt.RemoveAt(0);

        listInt = new List<int>();
        listInt.Add(item: 2);
        listInt.Add(item: 1);
        listInt.Add(item: 4);
        listInt.Add(item: 3);

        listInt.Sort();

        for (int i = 0; i < listInt.Count; i++)
        {
            Debug.Log(listInt[i]);
        }
    }

    void ArrayTest()
    {
        arrayInt = new int[4];

        arrayInt[0] = 3;
        arrayInt[1] = 0;
        arrayInt[2] = 1;
        arrayInt[3] = 2;

        for (int i = 0; i < arrayInt.Length; i++)
        {
            Debug.Log(arrayInt[i]);
        }

    }

    void Jagged2dArray()
    {
        jaggedArray2dOfInt = new int[4][];

        jaggedArray2dOfInt[0] = new int[2];
        jaggedArray2dOfInt[1] = new int[3];
        jaggedArray2dOfInt[2] = new int[3];
        jaggedArray2dOfInt[3] = new int[2];

        jaggedArray2dOfInt[0][0] = 1;

        for (int x = 0; x < jaggedArray2dOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray2dOfInt[x].Length; y++)
            {
                Debug.Log(jaggedArray2dOfInt[x][y]);
            }
        }
    }

    void Jagged3dArray()
    {
        jaggedArray3dOfInt = new int[3][][];

        jaggedArray3dOfInt[0] = new int[2][];
        jaggedArray3dOfInt[1] = new int[3][];
        jaggedArray3dOfInt[2] = new int[2][];

        jaggedArray3dOfInt[0][0] = new int[2];
        jaggedArray3dOfInt[0][1] = new int[3];


        for (int y = 0; y < jaggedArray3dOfInt[1].Length; y++)
        {
            jaggedArray3dOfInt[1][y] = new int[2];
        }

        for (int y = 0; y < jaggedArray3dOfInt[2].Length; y++)
        {
            jaggedArray3dOfInt[2][y] = new int[2];
        }

        

        for (int x = 0; x < jaggedArray3dOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray3dOfInt[x].Length; y++)
            {
                for (int z = 0; z < jaggedArray3dOfInt[x][y].Length; z++)
                {
                    Debug.Log(jaggedArray3dOfInt[x][y][z]);
                }
            }
        }

    }

    public void RaycastTest()
    {
        Vector3 pos = transform.position;
        Vector3 direction = Vector3.down;
        RaycastHit hit;
        float rayRange = 1f;

        if (Physics.Raycast(pos, direction, out hit, rayRange, layerMask))
        {
            Debug.DrawRay(pos, direction, Color.red);
        }
        else
        {
            Debug.DrawRay(pos, direction, Color.green);
        }
    }
}
