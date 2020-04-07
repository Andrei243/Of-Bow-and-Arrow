using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //TESTING
    public GameObject playerCam;
    //Variables
    public float speed = 0.05f;
    public float gravityForce = 0.1f;
    public float jumpForce = 2f;
    public float maxVerticalSpeed = 2f;

    //Objects
    public GameObject Archer;
    public Transform feetPoint;
    public Transform headPoint;
    public Transform rightPoint;
    public Transform leftPoint;


    //Movement
    private Transform archerTransform;
    private Animator animator;
    private bool facingRight;
    private bool isGrounded;

    //Gravity
    private float verticalForce;
    private RaycastHit hit;
    private Ray ray;

    //Collision
    private Vector3 safePoz;
    private bool leftOK, rightOK;
    private float leftDistance, rightDistance;

    void Start()
    {
        animator = Archer.GetComponent<Animator>();
        archerTransform = Archer.GetComponent<Transform>();

        facingRight = true;

    }

    void Update()
    {
        //FOR TESTING ONLY
        playerCam.transform.position = new Vector3(playerCam.transform.position.x, archerTransform.position.y, archerTransform.position.z);
        print(isGrounded);
       
        Movement();
        Gravity();
        Collisions();

        //Limit the speed
        verticalForce = Mathf.Min(verticalForce, maxVerticalSpeed);
        verticalForce = Mathf.Max(verticalForce, -maxVerticalSpeed);

        //Apply the force
        archerTransform.position = archerTransform.position + new Vector3(0, verticalForce, 0);
    }


    void Movement()
    {
        // MISCARE

        /* Pentru ca animatiile aratau gresit cand le puneam in oglinda a trebuit ca de fiecare data cand
         * perosnajul se intoarce sa inversez local scale-ul astfel incat animatiile sa afiseze caracterul 
         * cu fata spre camera. Deasemenea cand inversam local scale-ul imi invartea si caracterul pe Oz
         * (era cu capul in jos) asa ca am aplicat si o rotatie pentru a avea orientarea corecta */

        // Stanga
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (facingRight)
            {
                facingRight = false;
                archerTransform.localScale *= -1;
                archerTransform.rotation = new Quaternion(0, 0, 180, 1);
            }

            animator.SetFloat("Input Z", archerTransform.transform.position.z);
            animator.SetBool("Moving", true);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("Moving", false);

        }

        //Dreapta
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!facingRight)
            {
                facingRight = true;
                archerTransform.localScale *= -1;
                archerTransform.rotation = new Quaternion(0, 0, 0, 1);
            }
            animator.SetFloat("Input Z", archerTransform.transform.position.z);
            animator.SetBool("Moving", true);
            animator.SetBool("Mirror", false);

        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("Moving", false);
        }


        //Sarit
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                verticalForce += jumpForce * Time.deltaTime;
                isGrounded = false;
            }
        }
    }

    void Gravity()
    {
        ray = new Ray(feetPoint.position,Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            if (Vector3.Distance(feetPoint.position, hit.point) > 0.1)
                verticalForce -= gravityForce * Time.deltaTime;
        }
    }

    void Collisions()
    {
        rightOK = leftOK = true;

        //coliziune in sus
        ray = new Ray(headPoint.position, Vector3.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && Vector3.Distance(headPoint.position, hit.point) <=0.1)
                if (verticalForce > 0)
                    verticalForce = 0;
        }

        //coliziune in jos
        ray = new Ray(feetPoint.position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && Vector3.Distance(feetPoint.position, hit.point) <= 0.1)
            {
                if (verticalForce < 0)
                    verticalForce = 0;

                if (verticalForce <= 0)
                    isGrounded = true;
                
            }
        }

        //coliziune la stanga

        if (facingRight)
            leftDistance = 0.5f;
        else
            leftDistance = 1.4f;
        ray = new Ray(leftPoint.position, Vector3.back);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && Vector3.Distance(leftPoint.position, hit.point) <= leftDistance)
                leftOK = false;
        }

        //coliziune la dreapta

        /*In fucntie de ce directie priveste jucatorul punctele din stanga si dreapta isi schimba locatia
         *De aceea verific si ajustez distanta pentru a fi mereu constanta in functie de pozitia jucatorului
         *Asemanator si pentru coliziune la stanga
         */

        if (facingRight)
            rightDistance = 0.5f;
        else
            rightDistance = 1.4f;
        ray = new Ray(rightPoint.position, Vector3.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && Vector3.Distance(rightPoint.position, hit.point) <= rightDistance)
                rightOK = false;
        }

        if (rightOK && leftOK)
            safePoz = archerTransform.position;
        else
            archerTransform.position = new Vector3(archerTransform.position.x, archerTransform.position.y, safePoz.z);

    }
}



