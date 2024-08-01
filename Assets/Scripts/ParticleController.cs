using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem touchParticle;
    public Rigidbody2D playerRb;

    private int occurAfterVelocity;
    private float delayTime;

    private float counter;

    private void Start()
    {
        occurAfterVelocity = 1;
        delayTime = 0.2f;
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (Mathf.Abs(playerRb.velocity.x) > occurAfterVelocity)
        {
            Vector3 ls = movementParticle.transform.localScale;

            if (playerRb.velocity.x > 0)
                ls.x = Mathf.Abs(ls.x);
            else
                ls.x = -Mathf.Abs(ls.x);

            movementParticle.transform.localScale = ls;

            if (counter > delayTime)
            {
                if (PlayerController.Instance.GetIsGrounded())
                    movementParticle.Play();

                counter = 0;
            }
        }

        if (counter > delayTime)
        {
            if (PlayerController.Instance.GetIsFallDown())
            {
                fallParticle.Play();
                PlayerController.Instance.SetIsFallDown(false);
            }

            if (PlayerController.Instance.GetIsTouch())
            {
                touchParticle.Play();
                PlayerController.Instance.SetIsTouch(false);
            }

            counter = 0;
        }
    }
}
