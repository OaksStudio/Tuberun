using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private float Speed = 5;
    [SerializeField] private float Modifier = 1;
    [SerializeField] private Vector3 Direction = -Vector3.forward;
    [SerializeField] private Transform target;

    public void SetModifier(float modifier)
    {
        Modifier = modifier;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    private void Update()
    {
        target.Rotate(Direction * Speed * Modifier * Time.deltaTime);
    }
}
