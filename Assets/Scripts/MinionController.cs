using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinionController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    bool isTouched = false;
    bool isGrounded = true;

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
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rb;
    TargetRamps targetRamps;
    PlayerController player;
    ConfigurableJoint m_joint;
    ConfigurableJoint p_joint;
    JointDrive m_drive;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetRamps = FindObjectOfType<TargetRamps>();
        player = FindObjectOfType<PlayerController>();
        m_joint = gameObject.GetComponent<ConfigurableJoint>();
        p_joint = player.GetComponent<ConfigurableJoint>();
    }

    void Start()
    {
        runTowards = new Vector3(0, 0, speed);
        score = 0;
        CreateJointDrive();
    }

    void FixedUpdate()
    {
        Movement();
        Jump();
        Falling();

        Vector3 gravity = m_gravityScale * m_globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }


    void Movement()
    {
        if (player.isStacked)
        {
            transform.position += runTowards * Time.deltaTime;
        }
    }

    void Falling()
    {
        if (transform.position.y < 0)
        {
            player.isStacked = false;
            animator.SetTrigger("Death");
            BreakJoint();
        }
    }

    void CreateJointDrive()
    {
        m_drive = new JointDrive();
        m_drive.positionSpring = 1000;
    }

    void BreakJoint()
    {
        p_joint.connectedBody = null;
        player.joint.yMotion = ConfigurableJointMotion.Free;
        player.joint.zMotion = ConfigurableJointMotion.Free;
        player.joint.xDrive = player.drive0;
        player.joint.yDrive = player.drive0;
        player.joint.zDrive = player.drive0;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }


    void Jump()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded && player.isStacked)
        {
            isGrounded = false;
            rb.velocity = new Vector3(0, yForce, zForce);
            animator.SetTrigger("Jumping");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Running");
        }

        if (other.gameObject.tag == "Minion" && !player.isStacked)
        {
            player.isStacked = true;
            animator.Play("Dynamic Idle");
            m_joint.yMotion = ConfigurableJointMotion.Locked;
            m_joint.zMotion = ConfigurableJointMotion.Locked;
            m_joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
            m_joint.yDrive = m_drive;
            m_joint.xDrive = m_drive;
            m_joint.zDrive = m_drive;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && player.isStacked)
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
