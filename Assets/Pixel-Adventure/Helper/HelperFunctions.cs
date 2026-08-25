using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static IEnumerator WaitCurrentAnimationEnd(Animator animator, Action onCompleted)
    {
        // Chờ 1 frame để Animator kịp chuyển state
        yield return null;

        // Lấy thông tin State hiện tại
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Đợi cho đến khi normalizedTime >= 1.0f (nghĩa là đã chạy xong 100% độ dài clip)
        while (stateInfo.normalizedTime < 1.0f)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        // Tín hiệu hoàn tất!
        Debug.Log("Animation kết thúc từ Coroutine!");
        onCompleted?.Invoke();
    }

    public static void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]); // Cú pháp tráo đổi gọn của C#
        }
    }
}
