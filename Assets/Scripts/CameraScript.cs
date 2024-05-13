using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bar;

    public bool follow = false;
    private Vector2 currentPosition;

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (follow && transform.position.y < currentPosition.y + 19.5f)
        {
            var step = 5.2f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + step, -10);
        }
        else
        {
            currentPosition = transform.position;
            follow = false;
        }
    }
}
