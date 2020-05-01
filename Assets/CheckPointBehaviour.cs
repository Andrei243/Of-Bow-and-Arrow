using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public int ID { get; set; }
    public ParticleSystem System;
    public GameObject Child;

    public void ActivateParticleSystem()
    {
        var main = System.main;
        main.loop = false;
        System.Play();
    }
    void Start()
    {
        Child.GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            var gameObj = collision.gameObject;
            Destroy(gameObj);
        }
    }
}
