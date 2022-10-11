using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{

    [Range(0, 1), SerializeField] private float _distanceToGround;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _layerMask;

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator)
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat("IKLeftFootWeight"));
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat("IKLeftFootWeight"));
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _animator.GetFloat("IKRightFootWeight"));
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _animator.GetFloat("IKRightFootWeight"));

            //Left foot
            RaycastHit hit;
            Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if(Physics.Raycast(ray, out hit, _distanceToGround + 1f, _layerMask))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += _distanceToGround;
                    _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            //Right foot
            ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, _distanceToGround + 1f, _layerMask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += _distanceToGround;
                    _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}
