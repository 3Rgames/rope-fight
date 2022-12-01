using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
    public bool IsPlayerActive = true;
    [SerializeField] private Transform _animatedBody;
    [SerializeField] private Transform _physicalBody;

    private List<ConfigurableJoint> _configurableJoints = new List<ConfigurableJoint>();
    private List<Transform> _transforms = new List<Transform>();

    private List<Quaternion> _targetInitialRotations = new List<Quaternion>();

    private void OnEnable()
    {
        foreach (Transform physical in _physicalBody.GetComponentsInChildren<Transform>())
        {
            if (physical.TryGetComponent(out ConfigurableJoint joint))
            {
                foreach (Transform animated in _animatedBody.GetComponentsInChildren<Transform>())
                {
                    if (joint.name == animated.name)
                    {
                        _configurableJoints.Add(joint);
                        _transforms.Add(animated);
                        _targetInitialRotations.Add(animated.localRotation);
                        continue;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsPlayerActive)
            return;
        for (int i = 0; i < _configurableJoints.Count; i++)
            _configurableJoints[i].targetRotation = CopyRotation(_transforms[i], _targetInitialRotations[i]);
    }

    private Quaternion CopyRotation(Transform target, Quaternion targetQ)
    {
        return Quaternion.Inverse(target.localRotation) * targetQ;
    }

    public void DeleteJoints()
    {
        for (int i = 0; i < _configurableJoints.Count; i++)
            Destroy(_configurableJoints[i]);
    }
}
