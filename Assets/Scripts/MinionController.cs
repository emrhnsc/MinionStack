using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinionController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    bool isTouched = false;
    bool isGrounded = true;
    bool isStacked = false;

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
    [SerializeField] float m_gravityScale = 1f;
    [SerializeField] static float m_globalGravity = -9.81f;

    Vector3 runTowards;
    Animator animator;
    [HideInInspector] public Rigidbody rb;
    TargetRamps targetRamps;
    PlayerController playerController;
    ConfigurableJoint joint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetRamps = FindObjectOfType<TargetRamps>();
        playerController = FindObjectOfType<PlayerController>();
        joint = gameObject.GetComponent<ConfigurableJoint>();
    }

    void Start()
    {
        runTowards = new Vector3(0, 0, speed);
        score = 0;
    }

    void Update()
    {
        Movement();
        Jump();
    }

    void FixedUpdate()
    {
        Vector3 gravity = m_gravityScale * m_globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void Movement()
    {
        if (playerController.isStacked)
        {
            transform.position += runTowards * Time.deltaTime;
        }
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }


    void Jump()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded && playerController.isStacked)
        {
            isGrounded = false;
            rb.velocity = new Vector3(0, yForce, zForce);
            animator.SetTrigger("Jumping");
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && playerController.isStacked)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Running");
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
