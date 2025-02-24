using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerInput playerInput;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // private bool isGrounded = false;
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

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     ContactPoint2D contact = collision.contacts[0];
    //     //Debug.Log(collision.gameObject.tag);
    //     if (!collision.gameObject.CompareTag("Fruit"))
    //     {
    //         if (contact.normal.x != 0f)
    //         {
    //             //Debug.Log("Contact side");
    //             IsWallJump = true;
    //             animator.SetBool("IsWallJump", IsWallJump);

    //             isTouch = true;
    //         }

    //         if (contact.normal.y > 0f)
    //         {
    //             isGrounded = true;
    //             animator.SetFloat("yVelocity", 0f);
    //             jumpCount = 0;
    //             animator.SetFloat("jumpCount", 0f);
    //             animator.SetBool("IsJumping", !isGrounded);
    //             isFallDown = true;

    //             IsWallJump = false;
    //             animator.SetBool("IsWallJump", IsWallJump);
    //         }
    //     }

    //     else
    //     {
    //         Fruits fruit = collision.gameObject.GetComponent<Fruits>();
    //         fruit.SetIsCollected(true);
    //         //int fruitID = fruit.GetFruitID();
    //         //FruitManager.Instance.ReturnFruit(fruitID, fruit);
    //         //Debug.Log(fruitID);
    //     }

    //     if (collision.gameObject.CompareTag("Box"))
    //     {
    //         if (contact.normal.x == 0)
    //         {
    //             Boxes box = collision.gameObject.GetComponent<Boxes>();
    //             box.SetIsHit(true);

    //             if (contact.normal.y > 0)
    //             {
    //                 rb.AddForce(impulsePower * 0.6f, ForceMode2D.Impulse);
    //             }
    //             else
    //             {
    //                 rb.AddForce(-impulsePower * 0.35f, ForceMode2D.Impulse);
    //             }
    //         }
    //     }

    //     if (collision.gameObject.CompareTag("Trap"))
    //     {
    //         Trap trap = collision.gameObject.GetComponent<Trap>();
    //         trap.SetIsHit(true);
    //         if (trap.GetTrapID() == TrapsManager.Instance.GetTrapData().trampolineID)
    //         {
    //             rb.AddForce(impulsePower, ForceMode2D.Impulse);
    //             jumpCount++;
    //         }

    //         if (trap.GetTrapID() == TrapsManager.Instance.GetTrapData().blockID)
    //         {
    //             if (contact.normal.y > 0)
    //             {
    //                 rb.AddForce(impulsePower * 0.25f, ForceMode2D.Impulse);
    //             }
    //             else
    //             {
    //                 rb.AddForce(-impulsePower * 0.2f, ForceMode2D.Impulse);
    //             }
    //         }
    //         Debug.Log(trap.GetTrapID());
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Terrain"))
    //     {
    //         if (IsWallJump)
    //         {
    //             IsWallJump = false;
    //             animator.SetBool("IsWallJump", IsWallJump);
    //         }
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Fruit"))
    //     {
    //         //Fruits fruit = collision.gameObject.GetComponent<Fruits>();
    //         //int fruitID = fruit.GetFruitID();
    //         //FruitManager.Instance.ReturnFruit(fruitID, fruit);

    //         Fruits fruit = collision.gameObject.GetComponent<Fruits>();
    //         fruit.SetIsCollected(true);
    //     }
    // }

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Trap"))
    //     {
    //         Fan fan = collision.gameObject.GetComponent<Fan>();
    //         if (fan.GetTrapID() == TrapsManager.Instance.GetTrapData().fanID)
    //         {
    //             if (fan.GetToggle())
    //                 forcePower = fan.GetForcePower();
    //             else
    //                 forcePower = Vector2.zero;
    //         }
    //         Debug.Log("Collide with fan");
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Trap"))
    //     {
    //         Fan fan = collision.gameObject.GetComponent<Fan>();
    //         if (fan.GetToggle())
    //             forcePower = Vector2.zero;
    //     }
    // }

    // public void DoneDoubleJump()
    // {
    //     animator.SetFloat("jumpCount", 0f);
    //     Debug.Log("Done double jump");
    // }
}