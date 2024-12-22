using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
