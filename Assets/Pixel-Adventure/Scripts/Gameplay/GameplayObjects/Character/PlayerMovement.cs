using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private bool isFacingRight = true;

    public bool isGrounded = false;
    public bool isJumpInAir = false;

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
        Move();
        Jump();
    }

    private void Move()
    {
        playerRB.velocity = new Vector2(
            PlayerController.Instance.playerInput.move.x * moveSpeed,
            playerRB.velocity.y
        );
    }

    private void Jump()
    {
        if (PlayerController.Instance.playerInput.jump == true)
        {
            if (isGrounded == true)
            {
                playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                PlayerController.Instance.playerInput.jump = false;
            }

            if (isJumpInAir == false && isGrounded == false)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                
                playerRB.AddForce(Vector2.up * jumpAirPower, ForceMode2D.Impulse);
                isJumpInAir = true;
            }
        }
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
