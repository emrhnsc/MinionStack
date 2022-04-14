using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    bool isTouched = false;
    bool isGrounded;
    public bool isStacked = false;

    [Header("Jump")]
    [SerializeField] float yForce = 5f;
    [SerializeField] float zForce = 2f;

    [Header("Score")]
    [SerializeField] int redScore;
    [SerializeField] int orangeScore;
    [SerializeField] int goldScore;
    [HideInInspector] public bool redColliding;
    [HideInInspector] public bool orangeColliding;
    [HideInInspector] public bool goldColliding;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;

    [Header("Gravity")]
    [SerializeField] float gravityScale = 1f;
    [SerializeField] static float globalGravity = -9.81f;

    Vector3 runTowards;
    Animator animator;
    Rigidbody rb;
    MinionController minion;
    Empty empty;
    [HideInInspector] public ConfigurableJoint joint;
    [HideInInspector] public JointDrive drive;
    [HideInInspector] public JointDrive drive0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        minion = FindObjectOfType<MinionController>();
        empty = FindObjectOfType<Empty>();
        joint = gameObject.GetComponent<ConfigurableJoint>();
    }

    void Start()
    {
        runTowards = new Vector3(0, 0, speed);
        score = 0;
        NewJointDrive();
        ZeroJointDrive();
    }

    void FixedUpdate()
    {
        Movement();
        Jump();

        Vector3 gravity = gravityScale * globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void NewJointDrive()
    {
        drive = new JointDrive();
        drive.positionSpring = 1000;
    }

    void ZeroJointDrive()
    {
        drive0 = new JointDrive();
        drive0.positionSpring = 0;
    }

    void Movement()
    {
        transform.position += runTowards * Time.deltaTime;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }


    void Jump()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded)
        {
            isGrounded = false;
            rb.velocity = new Vector3(0, yForce, zForce);
            animator.SetTrigger("Jumping");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Minion" && !isStacked)
        {
            isStacked = true;
            animator.Play("Dynamic Idle");
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.yDrive = drive;
            joint.xDrive = drive;
            joint.zDrive = drive;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && !isStacked)
        {
            isGrounded = true;
            animator.SetTrigger("Running");
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (other.gameObject.tag == "RedRound" && !isTouched)
            {
                isTouched = true;
                redColliding = true;
                IncreaseScore(redScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (other.gameObject.tag == "OrangeRound")
            {
                orangeColliding = true;
                IncreaseScore(orangeScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (other.gameObject.tag == "GoldRound")
            {
                goldColliding = true;
                IncreaseScore(goldScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

    }

    private void OnCollisionExit(Collision other)
    {


        if (other.gameObject.tag == "RedRound")
        {
            redColliding = false;
        }
        else if (other.gameObject.tag == "OrangeRound")
        {
            orangeColliding = false;
        }
        else if (other.gameObject.tag == "GoldRound")
        {
            isTouched = false;
            goldColliding = false;
        }
    }

    public void IncreaseScore(int value)
    {
        score += value;
    }

    public void ShowScore()
    {
        scoreText.text = "Score: " + GetScore().ToString();
    }

    public int GetScore()
    {
        return score;
    }

}
