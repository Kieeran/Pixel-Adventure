using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaks : MonoBehaviour
{
    List<SingleBreak> breaks;
    public event Action OnAllBreaksDisappear;

    [SerializeField] float _minLifeTime = 2f;
    [SerializeField] float _maxLifeTime = 2.5f;
    [SerializeField] int _blinkCount = 4;
    [SerializeField] float _blinkInterval = 0.15f;
    [SerializeField] float explosionForce = 1f;
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

        centerPoint = GetMidpoint(
            breaks[0].transform.localPosition,
            breaks[1].transform.localPosition
        );
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

    void Reset()
    {
        foreach (var b in breaks)
        {
            b.Reset();
        }
    }

    void Explode(SingleBreak breakPiece)
    {
        Vector2 dir = ((Vector2)breakPiece.transform.localPosition - centerPoint).normalized;
        breakPiece.rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
        breakPiece.rb.AddTorque(UnityEngine.Random.Range(-30f, 30f));
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

    public Vector2 GetMidpoint(Vector2 a, Vector2 b)
    {
        return new Vector2((a.x + b.x) / 2f, (a.y + b.y) / 2f);
    }
}
