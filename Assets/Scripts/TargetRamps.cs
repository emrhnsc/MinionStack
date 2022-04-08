using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRamps : MonoBehaviour
{
    PlayerController player;
    [HideInInspector] public Material material;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        material = GetComponent<Renderer>().material;
    }
}
