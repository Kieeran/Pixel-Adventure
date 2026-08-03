using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    public static readonly int IdleStateHash = Animator.StringToHash("Idle");
    public static readonly int RunStateHash = Animator.StringToHash("Run");

    public void PlayAnimation(int stateHash, float layer = 0, float normalizedTime = 0f)
    {
        // Chạy thẳng State đó mà không cần Transition
        animator.Play(stateHash, (int)layer, normalizedTime);
    }
}
