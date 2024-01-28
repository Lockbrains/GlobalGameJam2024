using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmIK : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private Transform fishMan;
    [SerializeField] private float armMoveSpeed = 10;
    
    [Header("Left Arm")]
    [SerializeField] private Transform leftArmStart;
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform leftArmMid;
    [SerializeField] private Transform leftArmWrist;

    private float _forearmLength;
    private float _reararmLenth;
    // cache result for repeat IK computation
    private float _A;
    private float _B;

    private Vector3 _targetLastPos;

    private bool _leftGrip = false;
    private bool _rightGrip = false;

    private void Awake()
    {
        _forearmLength = Mathf.Sqrt(
            Mathf.Pow(leftArmMid.position.x - leftArmStart.position.x, 2) 
            + Mathf.Pow(leftArmMid.position.y - leftArmStart.position.y, 2));
        
        _reararmLenth = Mathf.Sqrt(
            Mathf.Pow(leftArmMid.position.x - leftArmWrist.position.x, 2) 
            + Mathf.Pow(leftArmMid.position.y - leftArmWrist.position.y, 2));

        leftArmTarget.position = leftArmWrist.position;

        _A = Mathf.Pow(_forearmLength, 2.0f) + Mathf.Pow(_reararmLenth, 2.0f);
        _B = 2.0f * _forearmLength * _reararmLenth;
    }

    private void Start()
    {
        Debug.Log($"1arm: {_forearmLength}, 2arm: {_reararmLenth}");
    }

    private void Update()
    {
        ArmInput();
        
        if (!_leftGrip && _targetLastPos != leftArmTarget.position) SolveIKLeftArm();
    }

    private void ArmInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _leftGrip = !_leftGrip;
            leftArmTarget.position = _leftGrip ? leftArmStart.position : leftArmWrist.position;
            _targetLastPos = leftArmTarget.position;
        }
        if (Input.GetKeyDown(KeyCode.RightShift)) _rightGrip = !_rightGrip;
        
        if (!_leftGrip)
        {
            leftArmTarget.position +=
                new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * armMoveSpeed;
        }

    }

    private void SolveIKLeftArm()
    {
        // solve for RR IK
        float targetX = -(leftArmTarget.position.x - leftArmStart.position.x);
        float targetY = leftArmTarget.position.y - leftArmStart.position.y;
        float theta2 = Mathf.Acos((Mathf.Pow(targetX, 2.0f) + Mathf.Pow(targetY, 2.0f) - _A) / _B);
        // TODO: Should check for elbow up/down config
        float ang1 = Mathf.Atan2(targetY, targetX);
        float ang2 = Mathf.Atan2(_forearmLength * Mathf.Sin(theta2), _reararmLenth + _forearmLength * Mathf.Cos(theta2));
        float theta1 = ang1 - ang2;
        Debug.Log($"1arm: {Mathf.Rad2Deg * theta1}, 2arm: {Mathf.Rad2Deg * theta2}");
        // apply back to rig, reverse change if NaN
        if (Double.IsNaN(ang1) || Double.IsNaN(ang2))
        {
            leftArmTarget.position = _targetLastPos;
            return;
        }
        leftArmStart.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * theta1, 0);
        leftArmMid.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * theta2, 0);

        _targetLastPos = leftArmTarget.position;
    }

    private void SolveIKLeftBody()
    {
        float targetX = -(leftArmTarget.position.x - leftArmStart.position.x);
        float targetY = leftArmTarget.position.y - leftArmStart.position.y;
        float theta2 = Mathf.Acos((Mathf.Pow(targetX, 2.0f) + Mathf.Pow(targetY, 2.0f) - _A) / _B);
        // TODO: Should check for elbow up/down config
        float ang1 = Mathf.Atan2(targetY, targetX);
        float ang2 = Mathf.Atan2(_forearmLength * Mathf.Sin(theta2), _reararmLenth + _forearmLength * Mathf.Cos(theta2));
        float theta1 = ang1 - ang2;
        Debug.Log($"1arm: {Mathf.Rad2Deg * theta1}, 2arm: {Mathf.Rad2Deg * theta2}");
        // apply back to rig, reverse change if NaN
        if (Double.IsNaN(ang1) || Double.IsNaN(ang2))
        {
            leftArmTarget.position = _targetLastPos;
            return;
        }
        leftArmStart.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * theta1, 0);
        leftArmMid.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * theta2, 0);

        _targetLastPos = leftArmTarget.position;
    }
}
