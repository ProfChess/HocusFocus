using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnim : MonoBehaviour
{
    private float animSpeed = 1.5f;
    private float animHeight = 0.3f;
    private Vector2 location;

    private void Start()
    {
        location = transform.position;
    }

    private void Update()
    {
        float yChange = Mathf.Sin(Time.time * animSpeed) * animHeight;

        transform.position = location + new Vector2(0f, yChange);
    }
}
