using UnityEngine;

public class SingleBreak : MonoBehaviour
{
    public SpriteRenderer breakRenderer;
    public Rigidbody2D rb;
    public Vector3 originPos;
    public Vector3 originRot;

    void OnValidate()
    {
        originPos = transform.localPosition;
        originRot = transform.localEulerAngles;

        rb = GetComponent<Rigidbody2D>();
        breakRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (breakRenderer) breakRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        transform.localPosition = originPos;
        transform.localEulerAngles = originRot;
        HelperFunctions.SetAlpha(breakRenderer, 1f);
    }
}
