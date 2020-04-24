using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    //The arrow has the right orientation
    public float speed = 1;


    void Update()
    {
        // Move the arrow
        transform.position += transform.right * Time.deltaTime * speed;
    }


}
