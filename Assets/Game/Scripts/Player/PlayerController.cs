using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [Space]
    [Header("Controlling")]
    [Space]
    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private float _sensitivity = 3f;
    [Space]
    [Header("Rope")]
    [SerializeField] private float _percentDifferentToTargetForce = 5f;
    [SerializeField] private float _maxSwipeSpeed = 500f;
    [SerializeField] private Rope _leftRope;
    [SerializeField] private Rope _rightRope;

    private float _swipeSpeed = 0;

    private void FixedUpdate()
    {
        Moving();
        Rotating();
        LastPosSetUp();
        SwipeSpeedCheck();
    }

    private void SwipeSpeedCheck()
    {
        foreach (Touch th in Input.touches)
        {
            _swipeSpeed = Mathf.Abs(th.deltaPosition.x / Time.deltaTime);
        }
    }

    private void Rotating()
    {
        if (JoyStick.Instance.Joy.Horizontal != 0f && JoyStick.Instance.Joy.Vertical != 0f)
        {
            float angle = Mathf.Atan2(JoyStick.Instance.Joy.Horizontal, JoyStick.Instance.Joy.Vertical) * Mathf.Rad2Deg;
            _rb.rotation = Quaternion.Lerp(_rb.rotation, Quaternion.Euler(Vector3.up * angle), Time.deltaTime * _sensitivity);

            if (!IsSame(_rb.rotation.y, Quaternion.Euler(Vector3.up * angle).y, _percentDifferentToTargetForce))
            {
                if (_swipeSpeed >= _maxSwipeSpeed)
                {
                    _rightRope.TargetForce(transform.right);
                    _leftRope.TargetForce(-transform.right);
                }
            }
        }
    }

    private bool IsSame(float first, float second, float percents)
    {
        first = Mathf.Abs(first);
        second = Mathf.Abs(second);

        float onePercent = first / 100f;
        float targetNumber1 = first + (onePercent * percents);
        float targetNumber2 = first - (onePercent * percents);

        if (second <= targetNumber1 && second >= targetNumber2)
            return true;

        return false;
    }

    private Vector3 _lastPos = Vector3.zero;
    private void LastPosSetUp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastPos = transform.position;
            _animator.speed = 0f;
        }
    }
    private bool _wasHit = false;
    float _speed = 0f;
    private void Moving()
    {
        InputData input = InputController.Instance.InputData;

        if (JoyStick.Instance.Joy.Horizontal != 0f && JoyStick.Instance.Joy.Vertical != 0f || input.IsTouching)
        {
            _wasHit = true;
            _animator.speed = Mathf.InverseLerp(0f, _movingSpeed, _speed);
            _animator.SetBool("Run", true);
            _speed = _movingSpeed * Mathf.Clamp01(Mathf.Abs(JoyStick.Instance.Joy.Horizontal) + Mathf.Abs(JoyStick.Instance.Joy.Vertical));

            _rb.velocity = new Vector3(transform.forward.x * _speed, _rb.velocity.y, transform.forward.z * _speed);
            _rightRope.BackForce(transform);
            _leftRope.BackForce(transform);
        }
        else if (_wasHit)
        {
            _wasHit = false;
            _animator.speed = 1f;
            _animator.SetBool("Run", false);

            if (Vector3.Distance(_lastPos, transform.position) <= 1f)
                return;
            _rightRope.ForwardForce(transform);
            _leftRope.ForwardForce(transform);

        }
    }
}
