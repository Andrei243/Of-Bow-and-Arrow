using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour
{
    public string arrowTag = "PlayerArrow";
    public float moveSpeed = 0.5f;
    public int health = 3;
    public Transform RightPatrolPoint;
    public GameObject hitArea;

    private Animator anim;
    private Collider2D hitAreaCol;
    private SpriteRenderer sprite;
    private Rigidbody2D enemyRigidbody;
    private float velocity;
    private Vector3 leftPatrolPoint;
    private Vector3 rightPatrolPoint;
    private bool facingRight;
    private bool isDead;
    private bool isAttacking;
    private int playerLayer;

    void Start()
    {
        leftPatrolPoint = transform.position;
        rightPatrolPoint = RightPatrolPoint.position;
        facingRight = true;
        isDead = false;
        isAttacking = false;
        playerLayer = 1 << 10;
        sprite = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        hitAreaCol = hitArea.GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Action();
    }

    private void Movement()
    {
        if (facingRight)
        {
            //Check if the enemy reached his right patrol point
            if (rightPatrolPoint.x <= transform.position.x)
            {
                Flip(true);
            }
            else
                velocity = moveSpeed;
        }
        else
        {
            //Check if the enemy reached his left patrol point
            if (leftPatrolPoint.x >= transform.position.x)
            {
                Flip(false);
            }
            else
                velocity = -moveSpeed;
        }

        if (!isDead && !isAttacking)
            enemyRigidbody.velocity = new Vector2(velocity, 0);
        else
            enemyRigidbody.velocity = Vector2.zero;

    }
    private void Action()
    {
        if (hitAreaCol.IsTouchingLayers(playerLayer))
        {
            isAttacking = true;
            anim.SetTrigger("attack");
        }
    }

    void Attack()
    {
        isAttacking = false;

        if (hitAreaCol.IsTouchingLayers(playerLayer))
            GameObject.Find("Player").GetComponent<PlayerScript>().Die();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(arrowTag))
        {
            health--;
            if (health == 0)
            {
                isDead = true;
                anim.SetBool("dead", true);
            }
            else
                anim.SetTrigger("hit");
        }
    }

    void DestroyDemon()
    {
        Destroy(gameObject);
    }
    void Flip(bool flip)
    {
        sprite.flipX = flip;
        facingRight = !flip;
        velocity = 0;
        if (flip)
            hitArea.transform.position += new Vector3(-0.3f, 0, 0);
        else
            hitArea.transform.position += new Vector3(0.3f, 0, 0);

    }
}
