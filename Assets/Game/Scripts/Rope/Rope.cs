using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Rope")]
    [Space]
    [SerializeField] private List<Rigidbody> _ropeParts = new List<Rigidbody>();
    private List<ConfigurableJoint> _ropeJoints = new List<ConfigurableJoint>();
    [SerializeField] private float _forcePowerForward = 20f;
    [SerializeField] private float _forcePowerBack = 5f;
    [SerializeField] private float _forcePowerTarget = 5f;
    [Space]
    [Header("Joints")]
    [Space]
    [SerializeField] private float _positionSpring = 1000f;
    [SerializeField] private float _positionDamper = 0;

    private void OnEnable()
    {
        for (int i = 0; i < _ropeParts.Count; i++)
        {
            _ropeJoints.Add(_ropeParts[i].GetComponent<ConfigurableJoint>());
        }
        for (int i = 0; i < _ropeJoints.Count; i++)
        {
            JointDrive jointDrive = new JointDrive();

            jointDrive = _ropeJoints[i].angularXDrive;
            jointDrive.positionSpring = _positionSpring;
            jointDrive.positionDamper = _positionDamper;
            _ropeJoints[i].angularXDrive = jointDrive;

            jointDrive = _ropeJoints[i].angularYZDrive;
            jointDrive.positionSpring = _positionSpring;
            jointDrive.positionDamper = _positionDamper;
            _ropeJoints[i].angularYZDrive = jointDrive;
        }
    }

    public void ForwardForce(Transform player)
    {
        float _forceControl = _forcePowerForward / _ropeParts.Count;

        for (int i = 0; i < _ropeParts.Count; i++)
        {
            _ropeParts[i].AddForce((player.up + player.forward) * (_forcePowerForward - _forceControl * i), ForceMode.VelocityChange);
        }
    }

    public void BackForce(Transform player)
    {
        float _forceControl = _forcePowerBack / _ropeParts.Count;

        for (int i = 0; i < _ropeParts.Count; i++)
        {
            _ropeParts[i].velocity = Vector3.Lerp(_ropeParts[i].velocity, Vector3.zero, Time.deltaTime);
            _ropeParts[i].AddForce(-player.forward * (_forcePowerBack - _forceControl * i), ForceMode.Acceleration);
        }
    }

    public void TargetForce(Vector3 target)
    {
        float _forceControl = _forcePowerTarget / _ropeParts.Count;

        for (int i = 0; i < _ropeParts.Count; i++)
        {
            _ropeParts[i].velocity = Vector3.Lerp(_ropeParts[i].velocity, Vector3.zero, Time.deltaTime);
            _ropeParts[i].AddForce(target * (_forcePowerTarget - _forceControl * i), ForceMode.Acceleration);
        }
    }



}
