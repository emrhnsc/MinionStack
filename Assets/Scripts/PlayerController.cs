using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isPlaying;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] static float globalGravity = -9.81f;

    Vector3 runTowards;
    [HideInInspector] public Animator animator;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        runTowards = new Vector3(0, 0, speed);
        animator = GetComponent<Animator>();
        isPlaying = true;
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
}
