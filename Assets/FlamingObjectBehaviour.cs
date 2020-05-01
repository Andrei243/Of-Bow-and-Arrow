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
    private List<GameObject> Flammables { get; set; }
    public void Start()
    {
        FireParticle.SetActive(false);
        WaterParticle.SetActive(false);
        state = ObjectState.Normal;
        Flammables = new List<GameObject>();

    }
    public void SetOnFire()
    {
        switch (state)
        {
            case ObjectState.Normal:
                state = ObjectState.OnFire;
                MomentBurnt = DateTime.Now;
                FireParticle.SetActive(true);
                Flammables.ForEach(e => e.GetComponent<FlamingObjectBehaviour>().SetOnFire());
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Flammable")
        {
            Flammables.Add(collision.gameObject);
        }
    }

}
