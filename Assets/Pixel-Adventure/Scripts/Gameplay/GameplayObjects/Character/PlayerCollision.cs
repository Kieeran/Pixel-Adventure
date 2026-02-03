using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            PlayerController.Instance.playerMovement.isGrounded = true;
            PlayerController.Instance.playerMovement.isJumpInAir = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            PlayerController.Instance.playerMovement.isGrounded = false;
        }
    }

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
}
