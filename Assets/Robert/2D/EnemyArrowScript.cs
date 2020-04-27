﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowScript : MonoBehaviour
{
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }
}
