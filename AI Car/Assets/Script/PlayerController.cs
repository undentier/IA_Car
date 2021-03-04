using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CarController carController;

    private void Update()
    {
        carController.horizontaleInput = Input.GetAxis("Horizontal");
        carController.verticaleInput = Input.GetAxis("Vertical");
    }
}
