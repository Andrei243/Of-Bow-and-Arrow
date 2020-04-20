using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingObjectBehaviour : MonoBehaviour
{
    public TimeSpan TimeUntilBurnt = new TimeSpan(0, 0, 10);
    // Start is called before the first frame update
    private ObjectState state;
    private DateTime MomentBurnt;
    void Start()
    {
        state = ObjectState.Normal;
    }
    public void SetOnFire()
    {
        switch (state)
        {
            case ObjectState.Normal:
                state = ObjectState.OnFire;
                MomentBurnt = DateTime.Now;
                this.gameObject.AddComponent<ParticleSystem>();
                var particleSystem = this.gameObject.GetComponent<ParticleSystem>();
                
                break;
        }
    }

    public void WetTheWall()
    {
        switch (state)
        {
            case ObjectState.Normal:
                state = ObjectState.Wet;
                break;
            case ObjectState.OnFire:
                state = ObjectState.OnFire;
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
