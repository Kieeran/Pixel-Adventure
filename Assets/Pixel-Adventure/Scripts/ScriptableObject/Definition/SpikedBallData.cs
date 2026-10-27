using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikedBallData", menuName = "Scriptable Objects/SpikedBallData")]
public class SpikedBallData : CustomData
{
    // Vị trí Ball so với root, lưu ở local space.
    public Vector2 ballOffset;

    // Cấu hình chế độ quay liên tục và chiều quay; nếu tắt rotateContinuously thì object lắc giữa hai biên.
    public bool rotateContinuously;
    public bool rotateClockwise;

    // Góc ban đầu riêng cho quay liên tục; các góc được tính tương đối với góc đặt root trên level.
    public float continuousStartAngle;

    // Tốc độ góc tối đa (độ/giây), hai biên lắc và gia tốc góc khi lắc (độ/giây²).
    public float rotationSpeed;
    public float swingStartAngle;
    public float swingEndAngle;
    public float swingAcceleration;

    public List<Vector2> chainWaypoints;
    public Vector2 rootPos;
    public float spacing;
    public Sprite chainSkin;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not SpikedBall spikedBall) return;

        // Khôi phục các giá trị đã thiết kế trên asset vào instance đang mở hoặc đang chơi.
        spikedBall.SetBallOffset(ballOffset);
        spikedBall.rotateContinuously = rotateContinuously;
        spikedBall.rotateClockwise = rotateClockwise;
        spikedBall.continuousStartAngle = continuousStartAngle;
        spikedBall.rotationSpeed = rotationSpeed;
        spikedBall.swingStartAngle = swingStartAngle;
        spikedBall.swingEndAngle = swingEndAngle;
        spikedBall.swingAcceleration = swingAcceleration;

        // Asset cũ hoặc chưa cấu hình chains có thể chưa có waypoint list.
        if (chainWaypoints != null)
        {
            spikedBall.SetChainsProperties(chainWaypoints, rootPos, spacing, chainSkin);
        }
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not SpikedBall spikedBall) return;

        // Chụp cấu hình hiện tại từ object trong Level Designer sang asset để lưu cùng level.
        ballOffset = spikedBall.ball.localPosition;
        rotateContinuously = spikedBall.rotateContinuously;
        rotateClockwise = spikedBall.rotateClockwise;
        continuousStartAngle = spikedBall.continuousStartAngle;
        rotationSpeed = spikedBall.rotationSpeed;
        swingStartAngle = spikedBall.swingStartAngle;
        swingEndAngle = spikedBall.swingEndAngle;
        swingAcceleration = spikedBall.swingAcceleration;

        chainWaypoints = new List<Vector2>(spikedBall.chains.waypoints);
        rootPos = spikedBall.chains.rootPos;
        spacing = spikedBall.chains.spacing;
        chainSkin = spikedBall.chains.chainSkin.sprite;
    }
}
