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

    public event Action OnAllBreaksDisappear;

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
}