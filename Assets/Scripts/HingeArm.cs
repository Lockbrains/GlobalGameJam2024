using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeArm : MonoBehaviour
{
    [SerializeField] private float armForce = 100;

    [SerializeField] private GameObject gripPointPrefab;

    [SerializeField] private GameObject body;
    private Rigidbody _bodyRb;
    
    [SerializeField] private GameObject leftHand;
    private Rigidbody _leftHandRb;
    private bool _leftGrip = false;
    private GameObject _leftGripPoint;

    [SerializeField] private GameObject rightHand;
    private Rigidbody _rightHandRb;
    private bool _rightGrip = false;
    private GameObject _rightGripPoint;

    private void Awake()
    {
        _leftHandRb = leftHand.GetComponent<Rigidbody>();
        _rightHandRb = rightHand.GetComponent<Rigidbody>();
        _bodyRb = body.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Grip();
        
        if (!_leftGrip)
            _leftHandRb.AddForceAtPosition(
                Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f)) * armForce, 
                leftHand.transform.position - leftHand.transform.right);
        else
            _bodyRb.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * armForce);
        
        if (!_rightGrip)
            _rightHandRb.AddForceAtPosition(
                Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0.0f)) * armForce, 
                rightHand.transform.position + rightHand.transform.right);
        else 
            _bodyRb.AddForce(new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0.0f) * armForce);
    }

    private void Grip()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            _rightGrip = !_rightGrip;
            if (_rightGrip)
            {
                _rightGripPoint = Instantiate(gripPointPrefab);
                _rightGripPoint.transform.position = rightHand.transform.position + rightHand.transform.right;
                HingeJoint joint = _rightGripPoint.GetComponent<HingeJoint>();
                joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
                joint.connectedBody = _rightHandRb;
            }
            else
            {
                Destroy(_rightGripPoint);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _leftGrip = !_leftGrip;
            if (_leftGrip)
            {
                _leftGripPoint = Instantiate(gripPointPrefab);
                _leftGripPoint.transform.position = leftHand.transform.position - leftHand.transform.right;
                HingeJoint joint = _leftGripPoint.GetComponent<HingeJoint>();
                joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
                joint.connectedBody = _leftHandRb;
            }
            else
            {
                Destroy(_leftGripPoint);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leftHand.transform.position - leftHand.transform.right, 0.2f);
    }
}
