using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float endX;
    RectTransform transition;
    Vector2 originPos;
    float currentSpeed;

    void Awake()
    {
        transition = GetComponent<RectTransform>();
        originPos = transition.anchoredPosition;
        currentSpeed = 0;
    }

    private void Start()
    {
        UIManager.Instance.OnPreButtonPressed += () =>
        {
            currentSpeed = speed;
        };

        UIManager.Instance.OnNextButtonPressed += () =>
        {
            currentSpeed = speed;
        };
    }

    void Update()
    {
        if (currentSpeed != 0)
        {
            Vector2 movement = Vector2.right * (speed * Time.deltaTime);
            transition.anchoredPosition += movement;

            if (transition.anchoredPosition.x >= 0)
            {
                LevelManager.Instance.SetIsReadyToLoad();
            }

            if (transition.anchoredPosition.x >= endX)
            {
                transition.anchoredPosition = originPos;
                currentSpeed = 0;
            }
        }
    }
}