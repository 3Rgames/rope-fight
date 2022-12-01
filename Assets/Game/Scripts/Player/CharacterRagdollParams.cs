using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartOfBody
{
    public string Name = "Null";
    public List<ConfigurableJoint> Joints = new List<ConfigurableJoint>();
    public float PositionSpring = 200f;
    public float PositionDamper = 0f;

    public PartOfBody()
    {
        Name = "Null";
        Joints = new List<ConfigurableJoint>();
        PositionSpring = 200f;
        PositionDamper = 0f;
    }
}

public class CharacterRagdollParams : MonoBehaviour
{
    [SerializeField] private List<PartOfBody> _bodyParts = new List<PartOfBody>();

    private void Start()
    {
        SetUpJoints();
    }

    private void SetUpJoints()
    {
        for (int i = 0; i < _bodyParts.Count; i++)
        {
            for (int j = 0; j < _bodyParts[i].Joints.Count; j++)
            {
                JointDrive jointDrive = _bodyParts[i].Joints[j].angularXDrive;
                jointDrive.positionSpring = _bodyParts[i].PositionSpring;
                jointDrive.positionDamper = _bodyParts[i].PositionDamper;
                _bodyParts[i].Joints[j].angularXDrive = jointDrive;

                jointDrive = _bodyParts[i].Joints[j].angularYZDrive;
                jointDrive.positionSpring = _bodyParts[i].PositionSpring;
                jointDrive.positionDamper = _bodyParts[i].PositionDamper;
                _bodyParts[i].Joints[j].angularYZDrive = jointDrive;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetUpJoints();
    }
#endif
}
