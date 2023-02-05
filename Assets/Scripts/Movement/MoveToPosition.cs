using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public float Speed = 5;
    public Transform TargetPosition;

    public bool StartMoving;

    private void Update()
    {
        if (!StartMoving) return;
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition.position, Speed * Time.deltaTime);
    }
}
