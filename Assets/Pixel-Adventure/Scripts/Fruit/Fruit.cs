using System;
using System.Collections;
using UnityEngine;

public class Fruit : PlacedObject
{
    private static readonly int IsCollectedHash = Animator.StringToHash("IsCollected");
    [SerializeField] Animator animator;

    public virtual void SetIsCollected(bool b)
    {
        animator.SetBool(IsCollectedHash, b);
        StartCoroutine(WaitAnimationEnd(() =>
        {
            PoolManager.Instance.Return(this);
        }));
    }

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
    }

    IEnumerator WaitAnimationEnd(Action onCompleted)
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

    public override void OnSpawn()
    {

    }

    public override void OnDespawn()
    {

    }
}