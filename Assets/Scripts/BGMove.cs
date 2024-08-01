using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BGMove : MonoBehaviour
{
    public GameObject BG1;
    public GameObject BG2;

    private float speed;
    private float size;

    private void Start()
    {
        speed = 2f;
        size = 24f;
        //speed = BackgroundManager.Instance.GetBackgroundSpeed();
        //size = BackgroundManager.Instance.GetBackgroundSize();
    }

    private void Update()
    {
        BG1.transform.position += new Vector3(0, -speed) * Time.deltaTime;
        BG2.transform.position += new Vector3(0, -speed) * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Background1"))
        {
            collision.gameObject.transform.position = BG2.transform.position + new Vector3(0, size, 0);
        }

        else if (collision.CompareTag("Background2"))
        {
            collision.gameObject.transform.position = BG1.transform.position + new Vector3(0, size, 0);
        }
    }
}