using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerInput playerInput;
    public PlayerMovement playerMovement;
    public PlayerCollision playerCollision;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    // private bool isFallDown = false;
    // private bool isTouch = false;
    // private int jumpCount = 0;
    // private bool IsWallJump = false;

    // private Vector2 impulsePower = new Vector2(0, 700);
    // private Vector2 forcePower = Vector2.zero;
    // public Rigidbody2D rb;
    // public Animator animator;
    // public FruitsData fruitsData;

    // [SerializeField]
    // private float SlideDown = -1f;

    // public List<RuntimeAnimatorController> controllers;
    // public List<Sprite> skins;

    // public SpriteRenderer spriteRenderer;

    // public bool GetIsGrounded() { return isGrounded; }
    // public bool GetIsTouch() { return isTouch; }
    // public void SetIsTouch(bool b) { isTouch = b; }
    // public bool GetIsFallDown() { return isFallDown; }
    // public void SetIsFallDown(bool b) { isFallDown = b; }
    // public PlayerController GetPlayer() { return this; }

    // private void Start()
    // {
    //     ChangeRandomSkin();
    // }
    // public void ChangeRandomSkin()
    // {
    //     int randomIndex = Random.Range(0, 4);
    //     animator.runtimeAnimatorController = controllers[randomIndex];
    //     spriteRenderer.sprite = skins[randomIndex];
    // }

    // private void Update()
    // {
    //     horizontalInput = Input.GetAxis("Horizontal");
    //     FlipSprite();

    //     //if (Input.GetButtonDown("Jump") && isGrounded)
    //     //{
    //     //    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    //     //    isGrounded = false;
    //     //    //animator.SetBool("IsJumping", !isGrounded);
    //     //}

    //     if (Input.GetButtonDown("Horizontal"))
    //     {
    //         IsWallJump = false;
    //         animator.SetBool("IsWallJump", IsWallJump);
    //     }

    //     if (Input.GetButtonDown("Jump") && jumpCount < 2)
    //     {
    //         IsWallJump = false;
    //         animator.SetBool("IsWallJump", IsWallJump);

    //         rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    //         isGrounded = false;
    //         jumpCount++;
    //         animator.SetBool("IsJumping", !isGrounded);

    //         if (jumpCount == 2)
    //         {
    //             animator.SetFloat("jumpCount", 1f);
    //             //animator.SetBool("IsDoubleJump", true);
    //             //Debug.Log("Double jump");
    //         }
    //     }

    //     animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    //     animator.SetFloat("yVelocity", rb.velocity.y);
    // }

    // private void FixedUpdate()
    // {
    //     if (forcePower != Vector2.zero)
    //     {
    //         rb.velocity = new Vector2(horizontalInput * moveSpeed, 0) + forcePower;
    //         //rb.velocity = forcePower;
    //     }
    //     else
    //     {
    //         if (IsWallJump)
    //         {
    //             //rb.velocity = new Vector2(0, SlideDown);
    //             rb.velocity = new Vector2(0, SlideDown);
    //         }
    //         else
    //         {
    //             rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    //         }
    //     }

    //     //Debug.Log("IsWallJump: " + IsWallJump + " Velocity: " + rb.velocity);
    //     //Debug.Log(rb.velocity);
    // }

    // public void DoneDoubleJump()
    // {
    //     animator.SetFloat("jumpCount", 0f);
    //     Debug.Log("Done double jump");
    // }
}