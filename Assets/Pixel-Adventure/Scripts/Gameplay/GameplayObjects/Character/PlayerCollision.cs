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
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Block"))
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
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Block"))
        {
            PlayerController.Instance.playerInput.isGrounded = false;
            PlayerController.Instance.playerInput.isOnWall = false;
        }
    }
}
