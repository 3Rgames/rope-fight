using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [SerializeField] private EnemyController _parent;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Segment>(out Segment segment))
        {
            if (!_parent.IsDead)
            {
                TapTicController.Instance.Vibro(TapTicController.TapTicType.Heavy);
                _rb.AddForce(collision.contacts[0].point * _parent.ForcePower, ForceMode.Acceleration);
                _parent.BloodSpawn(collision.contacts[0].point, Quaternion.LookRotation(transform.forward, collision.contacts[0].normal), Vector3.one);
                _parent.Rb.constraints = RigidbodyConstraints.None;
                _parent.IsDead = true;

                if (_parent.ColorCor == null)
                    _parent.ColorCor = StartCoroutine(_parent.ColorChangeCor());

                if (_parent.LayerCor == null)
                    _parent.LayerCor = StartCoroutine(_parent.LayerChangeCor());

                if (_parent.DeadCor == null)
                    _parent.DeadCor = StartCoroutine(_parent.DeadChangeCor());
            }
        }
    }
}
