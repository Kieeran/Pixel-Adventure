using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private bool isFacingRight = true;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        PlayerController.Instance.OnJump += OnJump;
    }

    private void Update()
    {
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        playerRB.linearVelocity = new Vector2(
            PlayerController.Instance.playerInput.move.x * moveSpeed,
            playerRB.linearVelocity.y
        );
    }

    void OnJump()
    {
        if (PlayerController.Instance.playerInput.isGrounded == true)
        {
            playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (PlayerController.Instance.playerInput.isJumpInAir == false && PlayerController.Instance.playerInput.isGrounded == false)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);

            playerRB.AddForce(Vector2.up * jumpAirPower, ForceMode2D.Impulse);
            PlayerController.Instance.playerInput.isJumpInAir = true;
            PlayerController.Instance.OnDoubleJump?.Invoke();
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
