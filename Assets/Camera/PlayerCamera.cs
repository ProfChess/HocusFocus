using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform playerLocation;
    public Vector3 offset;
    public float followSpeed = 5;

    //Camera Bounds
    public float yMax;
    public float yMin;
    public float xMax;
    public float xMin;

    private void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(playerLocation.position.x, playerLocation.position.y, -10f);
        if (newPos.y > yMax)
        {
            newPos.y = yMax;
        }
        if (newPos.y < yMin)
        {
            newPos.y = yMin;
        }
        if (newPos.x > xMax)
        {
            newPos.x = xMax;
        }
        if (newPos.x < xMin)
        {
            newPos.x = xMin;
        }
        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
