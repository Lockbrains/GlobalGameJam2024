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
    public GameObject _leftGripPoint;

    [SerializeField] private GameObject rightHand;
    private Rigidbody _rightHandRb;
    private bool _rightGrip = false;
    public GameObject _rightGripPoint;

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float zValueCorrectionForce = 10;
    
    private void Awake()
    {
        _leftHandRb = leftHand.GetComponent<Rigidbody>();
        _rightHandRb = rightHand.GetComponent<Rigidbody>();
        _bodyRb = body.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        LeftGrip();
        RightGrip();
    }

    private void Update()
    {
        Grip();
        MoveHandToSurface();
        
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

    private void MoveHandToSurface()
    {
        Vector3 leftCastPoint = leftHand.transform.position - leftHand.transform.right + leftHand.transform.up * 10.0f;
        Vector3 rightCastPint = rightHand.transform.position + rightHand.transform.right + rightHand.transform.up * 10.0f;

        if (Physics.Raycast(leftCastPoint, -leftHand.transform.up, out var hitLeft, Mathf.Infinity, raycastLayerMask))
        {
            Debug.DrawRay(leftCastPoint, -leftHand.transform.up * hitLeft.distance, Color.yellow);

            float distance = hitLeft.point.z - leftHand.transform.position.z;
            if (distance >= 1.5f)
            {
                _leftHandRb.AddForceAtPosition(
                    new Vector3(0.0f, 0.0f, (distance > 0.0f) ? zValueCorrectionForce : -zValueCorrectionForce), 
                    leftHand.transform.position - leftHand.transform.right);
            }
                
        }
        else
        {
            Debug.LogWarning("left hand raycast hit nothing");
        }
        // ------------
        if (Physics.Raycast(rightCastPint, -rightHand.transform.up, out var hitRight, Mathf.Infinity, raycastLayerMask))
        {
            Debug.DrawRay(rightCastPint, -rightHand.transform.up * hitLeft.distance, Color.red);

            float distance = hitRight.point.z - rightHand.transform.position.z;
            if (Mathf.Abs(distance) >= 1.5f)
            {
                _rightHandRb.AddForceAtPosition(
                    new Vector3(0.0f, 0.0f, (distance > 0.0f) ? zValueCorrectionForce : -zValueCorrectionForce), 
                rightHand.transform.position + rightHand.transform.right);
            }
        }
        else
        {
            Debug.LogWarning("right hand raycast hit nothing");
        }
    }

    private void Grip()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            RightGrip();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            LeftGrip();
        }
    }

    private void LeftGrip()
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

    private void RightGrip()
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leftHand.transform.position - leftHand.transform.right, 0.2f);
    }
}
