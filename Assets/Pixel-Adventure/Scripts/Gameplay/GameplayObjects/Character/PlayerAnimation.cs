using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public StateMachine AniStateMachine { get; private set; }
    public IdleAniState IdleAniState { get; private set; }
    public WalkAniState WalkAniState { get; private set; }

    public static readonly int RunStateHash = Animator.StringToHash("Run");

    void Awake()
    {
        InitAniFSM();
    }

    void InitAniFSM()
    {
        AniStateMachine = new StateMachine();
        IdleAniState = new IdleAniState(AniStateMachine);
        WalkAniState = new WalkAniState(AniStateMachine);

        AniStateMachine.Initialize(IdleAniState);
    }

    void Update()
    {
        AniStateMachine.Update();
    }

    public void PlayAnimation(int stateHash, float layer = 0, float normalizedTime = 0f)
    {
        // Chạy thẳng State đó mà không cần Transition
        animator.Play(stateHash, (int)layer, normalizedTime);
    }
}
