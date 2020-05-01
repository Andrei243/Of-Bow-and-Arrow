using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectState
{
    Normal,
    OnFire,
    Wet
};

public class ArrowScript : MonoBehaviour
{
    //The arrow has the right orientation
    public float speed = 1;
    public GameObject FireParticle;
    public GameObject WaterParticle;
    private ObjectState arrowState = ObjectState.Normal;
    private bool isPlatform = false;

    public void Start()
    {
        FireParticle.SetActive(false);
        WaterParticle.SetActive(false);
    }
    void Update()
    {
        if (!isPlatform)
        {   // Move the arrow
            transform.position += transform.right * Time.deltaTime * speed;
        }
    }
    private void FixPlatform()
    {
        isPlatform = true;
        gameObject.AddComponent<Rigidbody2D>();
        var rig = this.gameObject.GetComponent<Rigidbody2D>();
        transform.position += new Vector3(0.01f, 0, 0);
        rig.SetRotation(Quaternion.identity);
        rig.freezeRotation = true;
        rig.constraints = RigidbodyConstraints2D.FreezeAll;
        var ground = LayerMask.NameToLayer("Ground");
        gameObject.layer = ground;
        gameObject.AddComponent<PlatformEffector2D>();
        gameObject.AddComponent<BoxCollider2D>();
        var boxCollider = gameObject.GetComponents<BoxCollider2D>();
        boxCollider[0].usedByEffector = true;
        boxCollider[0].isTrigger = false;
        boxCollider[1].isTrigger = true;

    }

    private void SetOnFire()
    {
        if (arrowState != ObjectState.Wet)
        {
            arrowState = ObjectState.OnFire;
            if (FireParticle != null)
            {

                FireParticle.SetActive(true);
            }
        }
    }

    private void WetTheArrow()
    {
        arrowState = ObjectState.Wet;
        if (FireParticle != null)
        {
            FireParticle.SetActive(false);
        }
        if (WaterParticle!=null)
        {
            WaterParticle.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlatform && collision.gameObject.tag == "Flammable")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlatform)
        {
            if (collision.gameObject.tag == "Flaming")
            {
                switch (arrowState)
                {
                    case ObjectState.Normal:
                        SetOnFire();
                        break;
                }
            }
            else if (collision.gameObject.tag =="Wet")
            {
                WetTheArrow();
            }
            else if (collision.gameObject.tag == "Flammable")
            {
                var flameBehaviour = collision.gameObject.GetComponent<FlamingObjectBehaviour>();
                switch (arrowState)
                {
                    case ObjectState.OnFire:
                        flameBehaviour.SetOnFire();
                        Destroy(gameObject);
                        break;
                    case ObjectState.Wet:
                        flameBehaviour.WetTheWall();
                        Destroy(gameObject);
                        break;
                    case ObjectState.Normal:
                        FixPlatform();
                        break;
                }

            }
            else if (collision.gameObject.tag == "Wall")
            {
                switch (arrowState)
                {
                    case ObjectState.Normal:
                        FixPlatform();
                        break;
                    default:
                        Destroy(this.gameObject);
                        break;
                }
            }
            else if (collision.gameObject.tag == "Floor")
            {
                Destroy(this.gameObject);
            }
        }
    }


}
