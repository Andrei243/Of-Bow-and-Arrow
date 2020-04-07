using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject cameraP;
    public float moveSpeed = 4;
    public float jumpForce = 8;
    public GameObject arrow;

    private GameObject Player;
    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerBoxCollider;
    private CircleCollider2D playerCircleCollider;
    private SpriteRenderer playerSprite;
    private Animator anim;

    private Vector2 leftSpeed, rightSpeed, jumpSpeed, velocity;
    private bool isGrounded;
    private bool facingRight;
    private int groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject;
        playerTransform = Player.transform;
        playerRigidbody = Player.GetComponent<Rigidbody2D>();
        playerBoxCollider = Player.GetComponent<BoxCollider2D>();
        playerCircleCollider = Player.GetComponent<CircleCollider2D>();
        playerSprite = Player.GetComponent<SpriteRenderer>();
        anim = Player.GetComponent<Animator>();

        leftSpeed = Vector2.left * moveSpeed;
        rightSpeed = Vector2.right * moveSpeed;
        jumpSpeed = Vector2.up * jumpForce;

        isGrounded = false;
        groundLayer = 1<<8;
        facingRight = true;
    }

    void Update()
    {
        cameraP.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,cameraP.transform.position.z);
        State();
        Animations();
        Movement();
    }

    void State()
    {
        if (playerCircleCollider.IsTouchingLayers(groundLayer))
            isGrounded = true;
    }

    void Animations()
    {
        if (Input.GetKeyDown(KeyCode.A) && isGrounded)
            anim.SetBool("running", true);

        if (Input.GetKeyUp(KeyCode.A))
            anim.SetBool("running", false);

        if (Input.GetKeyDown(KeyCode.D) && isGrounded)
            anim.SetBool("running", true);

        if (Input.GetKeyUp(KeyCode.D))
            anim.SetBool("running", false);


    }

    void Movement()
    {
        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            if(facingRight)
            {
                Flip();
                facingRight = false;
            }
            velocity += leftSpeed;
        }


        if (Input.GetKey(KeyCode.D))
        {
            if (!facingRight)
            {
                Flip();
                facingRight = true;
            }
            velocity += rightSpeed;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            velocity += jumpSpeed;
            isGrounded = false;
        }

        playerRigidbody.velocity = new Vector2(velocity.x, velocity.y + playerRigidbody.velocity.y);

        if(Input.GetMouseButton(0))
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(playerTransform.position);
            Vector2 mouseOnScreen =Camera.main.ScreenToViewportPoint(Input.mousePosition);
            positionOnScreen -= new Vector2(0.5f, 0.5f);
            mouseOnScreen -= new Vector2(0.5f, 0.5f);
            print(positionOnScreen.ToString() + mouseOnScreen.ToString());
            float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
 
            Instantiate(arrow, playerTransform.position, Quaternion.Euler(0,0,angle + 145f));
        }
        
    }

    void Flip()
    {
        playerSprite.flipX = !playerSprite.flipX;
    }
}
