using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaks : MonoBehaviour
{
    List<Transform> breakTransform;
    List<SpriteRenderer> breakRenderer;
    List<(Vector3 pos, Vector3 rot)> breakOriginPosRot;

    [SerializeField] float _minLifeTime = 2f;
    [SerializeField] float _maxLifeTime = 2.5f;
    [SerializeField] int _blinkCount = 4;
    [SerializeField] float _blinkInterval = 0.15f;
    [SerializeField] float explosionForce = 1f;

    public event Action OnAllBreaksDisappear;

    Vector2 centerPoint;

    void Awake()
    {
        Init();
        gameObject.SetActive(false);
    }

    void Init()
    {
        breakTransform = new();
        breakRenderer = new();
        breakOriginPosRot = new();
        foreach (Transform tf in transform)
        {
            breakTransform.Add(tf);
            breakOriginPosRot.Add((tf.localPosition, tf.localEulerAngles));
            breakRenderer.Add(tf.GetComponentInChildren<SpriteRenderer>());
        }

        centerPoint = GetIntersectionPoint(
            breakTransform[0].localPosition,
            breakTransform[3].localPosition,
            breakTransform[1].localPosition,
            breakTransform[2].localPosition
        );
    }

    void Reset()
    {
        for (int i = 0; i < breakTransform.Count; i++)
        {
            breakTransform[i].gameObject.SetActive(true);
            breakTransform[i].localPosition = breakOriginPosRot[i].pos;
            breakTransform[i].localEulerAngles = breakOriginPosRot[i].rot;

            SetAlpha(breakRenderer[i], 1f);
        }
    }

    public void Enable()
    {
        Reset();
        gameObject.SetActive(true);

        int remaining = breakTransform.Count;
        for (int i = 0; i < breakTransform.Count; i++)
        {
            Explode(breakTransform[i]);

            // Lambda không copy giá trị i — nó giữ reference đến biến i
            // Vòng lặp chạy xong → i = Count
            // Tất cả callback cùng đọc i = Count → sai
            //
            // int index = i → tạo biến mới trên stack mỗi iteration
            // Lambda capture index — biến này không ai thay đổi nữa

            int index = i;
            StartCoroutine(LifeCycleRoutine(breakRenderer[index], () =>
            {
                breakTransform[index].gameObject.SetActive(false);
                remaining--;

                if (remaining == 0)
                {
                    OnAllBreaksDisappear?.Invoke();
                }
            }));
        }
    }

    void Explode(Transform breakPiece)
    {
        Vector2 dir = ((Vector2)breakPiece.localPosition - centerPoint).normalized;
        breakPiece.GetComponent<Rigidbody2D>().AddForce(dir * explosionForce, ForceMode2D.Impulse);
        breakPiece.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-5f, 5f));
    }

    IEnumerator LifeCycleRoutine(SpriteRenderer renderer, Action onComplete)
    {
        // --- Giai đoạn 1: tồn tại trong khoảng thời gian ngẫu nhiên ---
        float lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
        yield return new WaitForSeconds(lifeTime);

        // --- Giai đoạn 2: blink ---
        int count = 0;
        while (count < _blinkCount)
        {
            SetAlpha(renderer, 0f);
            yield return new WaitForSeconds(_blinkInterval);

            SetAlpha(renderer, 1f);
            yield return new WaitForSeconds(_blinkInterval);

            count++;
        }
        onComplete?.Invoke();
    }

    void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    Vector2 GetIntersectionPoint(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        // Vector hướng của 2 đoạn thẳng
        float rx = B.x - A.x;
        float ry = B.y - A.y;
        float sx = D.x - C.x;
        float sy = D.y - C.y;

        // Tích hướng (Cross Product) của 2 vector hướng
        float denominator = rx * sy - ry * sx;

        // Tính tỉ lệ t trên đoạn AB
        float t = ((C.x - A.x) * sy - (C.y - A.y) * sx) / denominator;

        // Trả về tọa độ giao điểm
        return new Vector2(A.x + t * rx, A.y + t * ry);
    }
}