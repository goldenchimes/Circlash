using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField]
    GameObject instructions;

    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void OnEnable()
    {
        instructions.SetActive(true);
    }

    void OnDisable()
    {
        if (gameObject.scene.isLoaded)
        {
            instructions.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("AcceptValue"))
        {
            enabled = false;
            player.AcceptValue();
        }
    }
}
