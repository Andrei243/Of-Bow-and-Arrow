using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyScript : MonoBehaviour
{
    public string arrowTag = "PlayerArrow";
    public float moveSpeed = 0.5f;
    public int health = 3;
    public float reloadTime = 0.5f;
    public Transform RightPatrolPoint;
    public GameObject hitArea;
    public int ID;
    public bool isDead;
    public GameObject enemyArrow;

    private Animator anim;
    private Collider2D hitAreaCol;
    private SpriteRenderer sprite;
    private Rigidbody2D enemyRigidbody;
    private float velocity;
    private Vector3 leftPatrolPoint;
    private Vector3 rightPatrolPoint;
    private bool facingRight;
    private bool isAttacking;
    private int playerLayer;
    private float refreshTimer;

    void Start()
    {
        leftPatrolPoint = transform.position;
        rightPatrolPoint = RightPatrolPoint.position;
        facingRight = true;
        isDead = false;
        isAttacking = false;
        playerLayer = 1 << 10;
        refreshTimer = 0;
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
        if (refreshTimer > 0)
            refreshTimer -= Time.deltaTime;

        if (hitAreaCol.IsTouchingLayers(playerLayer) && refreshTimer <= 0)
        {
            isAttacking = true;
            refreshTimer = reloadTime;
            anim.SetTrigger("shoot");
        }
        
    }

    void Attack()
    {
        isAttacking = false;
        Vector3 player = GameObject.Find("Player").transform.position;
        float angle = Mathf.Atan2(player.y - transform.position.y, player.x - transform.position.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = 360 + angle;

        if (facingRight && angle > 90 && angle < 270)
            Flip(true);

        if (!facingRight && !(angle > 90 && angle < 270))
            Flip(false);

        Instantiate(enemyArrow, transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(arrowTag))
        {
            Destroy(collision.gameObject);
            health--;
            if (health == 0)
            {
                isDead = true;
                anim.SetBool("dead", true);
            }

        }
    }

    void DestroyDemon()
    {
        GameObject.Find("Main Camera").GetComponent<GameManager>().AddDeadEnemy(ID);
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    void Flip(bool flip)
    {
        sprite.flipX = flip;
        facingRight = !flip;
        velocity = 0;
        if (flip)
            hitArea.transform.rotation = new Quaternion(0, 0, 180, 1);
        else
            hitArea.transform.rotation = new Quaternion(0, 0, 0, 1);

    }
}
