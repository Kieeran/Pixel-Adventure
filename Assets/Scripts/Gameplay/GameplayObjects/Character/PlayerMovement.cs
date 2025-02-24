using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isFacingRight = true;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        playerRB.velocity = new Vector2(
            PlayerController.Instance.playerInput.move.x * moveSpeed,
            playerRB.velocity.y
        );
    }

    private void UpdateSpriteDirection()
    {
        Vector2 _move = PlayerController.Instance.playerInput.move;
        if (isFacingRight && _move.x < 0f || !isFacingRight && _move.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}
