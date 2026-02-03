using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private float transitionSpeed = 0.32f;
    private bool isTransitioning;

    private void Start()
    {
        isTransitioning = false;
    }

    public void StartTransition()
    {
        isTransitioning = true;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            this.transform.position += new Vector3(transitionSpeed, 0, 0);
            if (this.transform.position.x > 48.5f)
            {
                isTransitioning = false;
                this.transform.position = new Vector3(-48.5f, 0, 0);
            }

            if (this.transform.position.x > 0)
            {
                LevelManager.Instance.SetIsReadyToLoad();
            }
        }

        //Debug.Log(transform.position.x);
        //Debug.Log(isTransitioning);
    }
}