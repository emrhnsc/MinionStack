using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isPlaying;

    Vector3 runTowards;
    Animator animator;

    void Start()
    {
        runTowards = new Vector3(0, 0, speed);
        animator = GetComponent<Animator>();
        isPlaying = true;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += runTowards * Time.deltaTime;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" && isPlaying)
        {
            animator.SetTrigger("Running");
        }
    }
}
