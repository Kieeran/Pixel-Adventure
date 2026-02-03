using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] float speed = 500f;
    RectTransform backGround1;
    RectTransform backGround2;
    float bgHeight;

    void Start()
    {
        if (transform.childCount < 2) return;

        backGround1 = transform.GetChild(0).GetComponent<RectTransform>();
        backGround2 = transform.GetChild(1).GetComponent<RectTransform>();

        bgHeight = backGround1.rect.height;
        backGround1.anchoredPosition = Vector2.zero;
        backGround2.anchoredPosition = new Vector2(
            backGround1.anchoredPosition.x,
            backGround1.anchoredPosition.y + bgHeight
        );
    }

    void Update()
    {
        if (backGround1 == null || backGround2 == null) return;

        Vector2 movement = Vector2.down * (speed * Time.deltaTime);
        backGround1.anchoredPosition += movement;
        backGround2.anchoredPosition += movement;

        CheckAndResetBackground(backGround1, backGround2);
    }

    void CheckAndResetBackground(RectTransform bg1, RectTransform bg2)
    {
        if (bg1.anchoredPosition.y > 0 || bg2.anchoredPosition.y > 0) return;

        if (bg1.anchoredPosition.y > bg2.anchoredPosition.y)
        {
            bg2.anchoredPosition = new Vector2(
                bg1.anchoredPosition.x,
                bg1.anchoredPosition.y + bgHeight
            );
        }
        else
        {
            bg1.anchoredPosition = new Vector2(
                bg2.anchoredPosition.x,
                bg2.anchoredPosition.y + bgHeight
            );
        }
    }
}
