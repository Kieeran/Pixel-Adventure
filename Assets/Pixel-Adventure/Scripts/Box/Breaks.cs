using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaks : MonoBehaviour
{
    List<SingleBreak> breaks;

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
        breaks = new();
        foreach (Transform tf in transform)
        {
            if (tf.TryGetComponent<SingleBreak>(out var b))
            {
                breaks.Add(b);
            }
        }

        // Chổ này set theo index cố định
        // Vì không làm logic check giao điểm
        // Chỉ có thứ tự này mới cho ra hai đoạn thẳng giao nhau
        // Yes, nó dơ :D
        centerPoint = GetIntersectionPoint(
            breaks[0].transform.localPosition,
            breaks[3].transform.localPosition,
            breaks[1].transform.localPosition,
            breaks[2].transform.localPosition
        );
    }

    void Reset()
    {
        foreach (var b in breaks)
        {
            b.Reset();
        }
    }

    public void Enable()
    {
        Reset();
        gameObject.SetActive(true);

        int remaining = breaks.Count;
        for (int i = 0; i < breaks.Count; i++)
        {
            Explode(breaks[i]);

            // Lambda không copy giá trị i — nó giữ reference đến biến i
            // Vòng lặp chạy xong → i = Count
            // Tất cả callback cùng đọc i = Count → sai
            //
            // int index = i → tạo biến mới trên stack mỗi iteration
            // Lambda capture index — biến này không ai thay đổi nữa

            int index = i;
            StartCoroutine(LifeCycleRoutine(breaks[index].breakRenderer, () =>
            {
                breaks[index].gameObject.SetActive(false);
                remaining--;

                if (remaining == 0)
                {
                    OnAllBreaksDisappear?.Invoke();
                }
            }));
        }
    }

    void Explode(SingleBreak breakPiece)
    {
        Vector2 dir = ((Vector2)breakPiece.transform.localPosition - centerPoint).normalized;
        breakPiece.rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
        breakPiece.rb.AddTorque(UnityEngine.Random.Range(-5f, 5f));
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
            HelperFunctions.SetAlpha(renderer, 0f);
            yield return new WaitForSeconds(_blinkInterval);

            HelperFunctions.SetAlpha(renderer, 1f);
            yield return new WaitForSeconds(_blinkInterval);

            count++;
        }
        onComplete?.Invoke();
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