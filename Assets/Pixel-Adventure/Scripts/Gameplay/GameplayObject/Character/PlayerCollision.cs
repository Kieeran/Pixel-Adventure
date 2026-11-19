using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController playerController;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain") ||
            collision.gameObject.CompareTag("Box") ||
            collision.gameObject.CompareTag("Block") ||
            collision.gameObject.CompareTag("OneWayPlatform") ||
            collision.gameObject.CompareTag("RockHead")
        )
        {
            // Nếu va chạm với OneWayPlatform mà đang bị overlapped
            // => Hầu hết bỏ qua các logic tính toán đặc biệt phía dưới, maybe =))
            if (collision.gameObject.CompareTag("OneWayPlatform"))
            {
                ColliderDistance2D d = Physics2D.Distance(collision.collider, collision.otherCollider);
                if (d.distance < -0.05f)
                {
                    return;
                }
            }

            int count = 0;
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint2D contact = collision.GetContact(i);

                if (contact.normal.x > 0) playerController.playerInput.isContactLeftWall = true;
                else if (contact.normal.x < 0) playerController.playerInput.isContactLeftWall = false;

                // Nếu contact.normal.y > 0.7f => va chạm từ trên xuống => character chắn chắn đang đứng ở mặt đất
                // Set isGrounded = true
                // Set isOnWall = false
                // Set isJumpInAir = false (reset)
                if (contact.normal.y > 0.7f)
                {
                    playerController.playerInput.isGrounded = true;
                    playerController.playerInput.isOnWall = false;
                    playerController.playerInput.isJumpInAir = false;
                    break;
                }

                // Va chạm từ dưới lên => chạm trần => không làm gì cả
                if (contact.normal.y < -0.7f) break;

                count++;
            }

            // Nếu count = collision.contactCount - 1 => duyệt hết tất cả contact point rồi nhưng chưa chứng minh được 
            // character đang đứng ở mặt đất hay chạm trần (xét va chạm vertical)
            // => Character đang va chạm với tường (xét va chạm horizontal)
            if (count >= collision.contactCount - 1)
            {
                playerController.playerInput.isGrounded = false;
                // Chỉ khi character rớt xuống mà lúc đó đang va chạm với tường thì mới được tính là đang trên tường
                if (playerController.playerPhysic.playerRB.linearVelocityY <= 0f)
                {
                    // Sau tất cả các điều kiện thì chắn chắn player đang trượt trên tường rồi
                    // Nhưng nếu lúc này bị dính external push thì vẫn phải bỏ qua 
                    if (!playerController.playerInput.isExternallyForced)
                    {
                        playerController.playerInput.isOnWall = true;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain") ||
            collision.gameObject.CompareTag("Box") ||
            collision.gameObject.CompareTag("Block") ||
            collision.gameObject.CompareTag("OneWayPlatform") ||
            collision.gameObject.CompareTag("RockHead")
        )
        {
            playerController.playerInput.isGrounded = false;
            playerController.playerInput.isOnWall = false;
        }
    }
}
