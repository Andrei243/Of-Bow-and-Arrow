using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    public string[] wallTags = { "Flammable" };
    public string arrowTag = "PlayerArrow";
    public float moveSpeed = 0.5f;
    public int health = 3;
    public float reloadTime = 0.5f;
    public Transform RightPatrolPoint;
    public GameObject hitArea;
    public int ID;
    public bool isDead;

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
        //Checks if the enemy has reached a patrol point flips him if so
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

        //If the enemy is not dead or attacking then he can move
        if (!isDead && !isAttacking)
            enemyRigidbody.velocity = new Vector2(velocity, 0);
        else
            enemyRigidbody.velocity = Vector2.zero;

    }


    private void Action()
    {
        //If the player is in the enemy sight then start the attack animation
        if (refreshTimer > 0)
            refreshTimer -= Time.deltaTime;

        if (hitAreaCol.IsTouchingLayers(playerLayer) && refreshTimer <= 0)
        {
            isAttacking = true;
            refreshTimer = reloadTime;
            anim.SetTrigger("attack");
        }
    }

    void Attack()
    {
        //Called by an Action Event on the frame of the actual attack in "attack" animation.
        //If the player is still in the sight of the enemy then kill the player
        isAttacking = false;

        if (hitAreaCol.IsTouchingLayers(playerLayer))
            GameObject.Find("Player").GetComponent<PlayerScript>().Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the enemy touches a wall, flip him
        for(int i =0;i<wallTags.Length;i++)
            if(collision.gameObject.tag.Equals(wallTags[i]))
            {
                if (facingRight)
                    Flip(true);
                else
                    Flip(false);
            }

        //If the enemy touches a player arrow HP goes down, at 0 HP he dies
        if (collision.gameObject.tag.Equals(arrowTag))
        {
            Destroy(collision.gameObject);
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
        GameObject.Find("Main Camera").GetComponent<GameManager>().AddDeadEnemy(ID);
        gameObject.SetActive(false);
       // Destroy(gameObject);
    }
    void Flip(bool flip)
    {
        //Flip the enemy to the opposite direction he is facing 
        sprite.flipX = flip;
        facingRight = !flip;
        velocity = 0;
        if (flip)
            hitArea.transform.position += new Vector3(-0.3f, 0, 0);
        else
            hitArea.transform.position += new Vector3(0.3f, 0, 0);

    }
}
