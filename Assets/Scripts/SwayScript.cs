using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayScript : MonoBehaviour
{
    Vector3 startAngle;   //Reference to the object's original angle values

    float rotationSpeed = .0015f;  //Speed variable used to control the animation

    float rotationOffset = 1.5f; //Rotate by 50 units

    float finalAngle;  //Keeping track of final angle to keep code cleaner

    void Start()
    {
        startAngle = transform.rotation.eulerAngles;  // Get the start position
    }


    // Update is called once per frame
    void Update()
    {
        finalAngle = startAngle.z + (float)Math.Sin(Environment.TickCount * rotationSpeed) * rotationOffset;  //Calculate animation angle
        transform.rotation = Quaternion.Euler(startAngle.x, startAngle.y, finalAngle); //Apply new angle to object
    }

}
