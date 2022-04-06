using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRamps : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] float yForce = 5f;
    [SerializeField] float zForce = 2f;

    [Header("Rounds")]
    [SerializeField] bool isRed;
    [SerializeField] bool isOrange;
    [SerializeField] bool isGold;

    PlayerController player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.rigidbody.velocity = new Vector3(0, yForce, zForce);
            player.animator.SetTrigger("Jumping");
        }
    }
}
