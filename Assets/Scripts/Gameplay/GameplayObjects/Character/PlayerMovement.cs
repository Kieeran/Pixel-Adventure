using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerRB.velocity = new Vector2(
            PlayerController.Instance.playerInput.move.x * moveSpeed,
            playerRB.velocity.y
        );
    }
}
