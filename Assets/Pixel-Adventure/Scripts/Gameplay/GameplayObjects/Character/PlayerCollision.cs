using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            int count = 0;
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint2D contact = collision.GetContact(i);

                if (contact.normal.x > 0) PlayerController.Instance.playerInput.isContactLeftWall = true;
                else if (contact.normal.x < 0) PlayerController.Instance.playerInput.isContactLeftWall = false;

                // Nếu contact.normal.y > 0.7f thì character chắn chắn đang đứng ở mặt đất
                // Set isGrounded = true
                // Set isOnWall = false
                // Set isJumpInAir = false (reset)
                if (contact.normal.y > 0.7f)
                {
                    PlayerController.Instance.playerInput.isGrounded = true;
                    PlayerController.Instance.playerInput.isOnWall = false;
                    PlayerController.Instance.playerInput.isJumpInAir = false;
                    break;
                }

                // Chạm trần => không làm gì cả
                if (contact.normal.y < -0.7f) break;

                count++;
            }

            // Nếu count = collision.contactCount - 1 => duyệt hết tất cả contact point rồi nhưng chưa chứng minh được 
            // character đang đứng ở mặt đất hay chạm trần (xét va chạm vertical)
            // => Character đang va chạm với tường (xét va chạm horizontal)
            if (count >= collision.contactCount - 1)
            {
                PlayerController.Instance.playerInput.isGrounded = false;
                // Chỉ khi character rớt xuống mà lúc đó đang va chạm với tường thì mới được tính là đang trên tường
                if (PlayerController.Instance.playerMovement.playerRB.linearVelocityY < 0f)
                {
                    PlayerController.Instance.playerInput.isOnWall = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            PlayerController.Instance.playerInput.isGrounded = false;
            PlayerController.Instance.playerInput.isOnWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.CompareTag("Fruit"))
        {
            Fruit fruit = collision.gameObject.GetComponentInParent<Fruit>();
            fruit.SetIsCollected(true);
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
