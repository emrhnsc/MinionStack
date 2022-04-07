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

    [Header("Score")]
    [SerializeField] int redScore;
    [SerializeField] int orangeScore;
    [SerializeField] int goldScore;

    PlayerController player;
    Material material;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        material = GetComponent<Renderer>().material;
    }

    void PlayerJump(Collision value)
    {
        value.rigidbody.velocity = new Vector3(0, yForce, zForce);
        player.animator.SetTrigger("Jumping");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Mouse0))
        {
            if (isRed)
            {
                PlayerJump(other);
                player.IncreaseScore(redScore);
                player.ShowScore();
                material.color = Color.white;
            }
            else if (isOrange)
            {
                PlayerJump(other);
                player.IncreaseScore(orangeScore);
                player.ShowScore();
                material.color = Color.white;
            }
            else if (isGold)
            {
                PlayerJump(other);
                player.IncreaseScore(goldScore);
                player.ShowScore();
                material.color = Color.white;
            }
        }
    }

}
