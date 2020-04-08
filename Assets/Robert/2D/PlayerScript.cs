using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject cameraP;
    public float moveSpeed = 4;
    public float jumpForce = 8;
    public float reloadSpeed = 5;
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
    private int mouseClickLayer;
    private float reloadTimer;

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
        groundLayer = 1 << 8;
        mouseClickLayer = 1 << 9;
        facingRight = true;
        reloadTimer = 0;
    }

    void Update()
    {
        cameraP.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,cameraP.transform.position.z);
        State();
        Animations();
        Actions();

        if(Input.GetKeyDown(KeyCode.U))
        {
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
            anim.SetBool("dead", true);
        }
    }

    void State()
    {
        if (playerCircleCollider.IsTouchingLayers(groundLayer))
            isGrounded = true;
        if (reloadTimer > 0)
            reloadTimer -= Time.deltaTime;
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

    IEnumerator Shoot(float time, float angle)
    {
         yield return new WaitForSeconds(time);
        Instantiate(arrow, playerTransform.position, Quaternion.Euler(0, 0, angle));

    }

    void Actions()
    {

        if (Input.GetMouseButton(0) && reloadTimer <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float angle = 0;
            if (Physics.Raycast(ray, out hit, mouseClickLayer))
                angle = Mathf.Atan2(hit.point.y - transform.position.y, hit.point.x - transform.position.x) * Mathf.Rad2Deg;
            if (angle < 0)
                angle = 360 + angle;

            if (facingRight && angle > 90 && angle < 270)
                Flip(true);

            if (!facingRight && !(angle > 90 && angle < 270))
                Flip(false);

            anim.SetTrigger("shoot");
            StartCoroutine(Shoot(anim.GetCurrentAnimatorStateInfo(0).length, angle));
            reloadTimer = reloadSpeed + anim.GetCurrentAnimatorStateInfo(0).length;

        }

        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            if(facingRight)
                Flip(true);

            velocity += leftSpeed;
        }


        if (Input.GetKey(KeyCode.D))
        {
            if (!facingRight)
                Flip(false);
            
            velocity += rightSpeed;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            velocity += jumpSpeed;
            isGrounded = false;
        }

        playerRigidbody.velocity = new Vector2(velocity.x, velocity.y + playerRigidbody.velocity.y);

        
        
    }

    void Flip(bool flip)
    {
        playerSprite.flipX = flip;
        facingRight = !playerSprite.flipX;
    }
}
