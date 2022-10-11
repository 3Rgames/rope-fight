using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 1f;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.fixedDeltaTime * _speed);
    }
}
