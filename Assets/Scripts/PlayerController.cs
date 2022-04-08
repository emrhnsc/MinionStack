using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isPlaying;
    bool isTouched = false;

    [Header("Jump")]
    [SerializeField] float yForce = 5f;
    [SerializeField] float zForce = 2f;

    [Header("Score")]
    [SerializeField] int redScore;
    [SerializeField] int orangeScore;
    [SerializeField] int goldScore;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;

    [Header("Gravity")]
    [SerializeField] float gravityScale = 1f;
    [SerializeField] static float globalGravity = -9.81f;

    Vector3 runTowards;
    Animator animator;
    Rigidbody rb;
    TargetRamps targetRamps;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetRamps = FindObjectOfType<TargetRamps>();
    }

    void Start()
    {
        runTowards = new Vector3(0, 0, speed);
        isPlaying = true;
        score = 0;
    }

    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        Vector3 gravity = gravityScale * globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
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
        rb.velocity = new Vector3(0, yForce, zForce);
        animator.SetTrigger("Jumping");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && isPlaying)
        {
            animator.SetTrigger("Running");
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (other.gameObject.tag == "RedRound" && !isTouched)
            {
                Debug.Log("RED!");
                isTouched = true;
                Jump();
                IncreaseScore(redScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (other.gameObject.tag == "OrangeRound" && !isTouched)
            {
                Debug.Log("ORANGE!");
                isTouched = true;
                Jump();
                IncreaseScore(orangeScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (other.gameObject.tag == "GoldRound" && !isTouched)
            {
                Debug.Log("GOLD!");
                isTouched = true;
                Jump();
                IncreaseScore(goldScore);
                ShowScore();
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
        isTouched = false;
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
