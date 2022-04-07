using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isPlaying;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;

    [Header("Gravity")]
    [SerializeField] float gravityScale = 1f;
    [SerializeField] static float globalGravity = -9.81f;

    Vector3 runTowards;
    [HideInInspector] public Animator animator;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && isPlaying)
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
