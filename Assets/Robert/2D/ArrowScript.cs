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
    private ObjectState arrowState = ObjectState.Normal;
    private bool isPlatform = false;

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
        rig.SetRotation(Quaternion.identity);
        rig.freezeRotation = true;
        rig.constraints = RigidbodyConstraints2D.FreezeAll;
        var ground = LayerMask.NameToLayer("Ground");
        gameObject.layer = ground;
        gameObject.AddComponent<PlatformEffector2D>();
        var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.usedByEffector = true;
        boxCollider.isTrigger = false;

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
                        arrowState = ObjectState.OnFire;
                        break;
                }
            }
            else if (collision.gameObject.tag == "Wet")
            {
                arrowState = ObjectState.Wet;
            }
            else if (collision.gameObject.tag == "Flammable")
            {
                var flameBehaviour = collision.gameObject.GetComponent<FlamingObjectBehaviour>();
                switch (arrowState)
                {
                    case ObjectState.OnFire:
                        flameBehaviour.SetOnFire();
                        break;
                    case ObjectState.Wet:
                        flameBehaviour.WetTheWall();
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
        else
        {
            if (collision.gameObject.tag == "Player")
            {
            }
        }
    }


}
