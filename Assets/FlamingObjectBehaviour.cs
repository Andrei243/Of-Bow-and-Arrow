using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingObjectBehaviour : MonoBehaviour
{
    public int SecondsFromBurntTillDestroyed = 10;
    public GameObject FireParticle;
    public GameObject WaterParticle;
    // Start is called before the first frame update
    private ObjectState state;
    private DateTime MomentBurnt;
    public void Start()
    {
        FireParticle.SetActive(false);
        WaterParticle.SetActive(false);
        state = ObjectState.Normal;

    }
    public void SetOnFire()
    {
        switch (state)
        {
            case ObjectState.Normal:
                state = ObjectState.OnFire;
                MomentBurnt = DateTime.Now;
                FireParticle.SetActive(true);
                break;
        }
    }

    public void WetTheWall()
    {
        switch (state)
        {
            case ObjectState.Normal:
            case ObjectState.OnFire:
                state = ObjectState.Wet;
                FireParticle.SetActive(false);
                WaterParticle.SetActive(true);
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(state == ObjectState.OnFire)
        {
            if((DateTime.Now - MomentBurnt).TotalSeconds > SecondsFromBurntTillDestroyed)
            {
                Destroy(gameObject);
            }
        }
    }
}
