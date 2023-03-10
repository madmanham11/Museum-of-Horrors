using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class Hand : MonoBehaviour
{
    [Space]
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotateSpeed;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] private float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] LayerMask interactibleLayer;
    private Transform _followTarget;
    private Rigidbody _body;
    private bool _isGrabbing;
    private GameObject _heldObject;
    private Transform _grabPoint;
    private FixedJoint _joint1, _joint2;
    // Start is called before the first frame update
    void Start()
    {
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;

        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        //_body.velocity = (positionWithOffset - transform.position) / Time.fixedTime;

        // Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        if (Mathf.Abs(axis.magnitude) != Mathf.Infinity)
        {
            if (angle > 180.0f) { angle -= 360.0f; }
            _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed * Time.deltaTime);
        }
    }

    private void Grab(InputAction.CallbackContext context)
    {
        if(_isGrabbing || _heldObject)
        {
            return;
        }
        Collider[] grabbablecolliders = Physics.OverlapSphere(palm.position, reachDistance, interactibleLayer);
        if (grabbablecolliders.Length < 1)
        {
            return;
        }
        var objectToGrab = grabbablecolliders[0].transform.gameObject;

        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if(objectBody != null)
        {
            _heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent< Rigidbody >();
            if (objectBody != null)
            {
                _heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }
        StartCoroutine(GrabObject(grabbablecolliders[0], objectBody));
    }
    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        _isGrabbing = true;

        // Create a grab point
        _grabPoint = new GameObject().transform;
        _grabPoint.position = collider.ClosestPoint(palm.position);
        _grabPoint.parent = _heldObject.transform;

        //Move hand to grab point
        _followTarget = _grabPoint;

        // Wait for hand to reach grab point
        while (Vector3.Distance(_grabPoint.position, palm.position) > joinDistance && _isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        // Freeze hand and object motion
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
        //_body.rotation = Quaternion.;
        targetBody.velocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        // Attach joints
        _joint1 = gameObject.AddComponent<FixedJoint>();
        _joint1.connectedBody = targetBody;
        _joint1.breakForce = float.PositiveInfinity;
        _joint1.breakTorque = float.PositiveInfinity;

        _joint1.connectedMassScale = 1;
        _joint1.massScale = 1;
        _joint1.enableCollision = false;
        _joint1.enablePreprocessing = false;

        _joint2 = _heldObject.AddComponent<FixedJoint>();
        _joint2.connectedBody = _body;
        _joint2.breakForce = float.PositiveInfinity;
        _joint2.breakTorque = float.PositiveInfinity;

        _joint2.connectedMassScale = 1;
        _joint2.massScale = 1;
        _joint2.enableCollision = false;
        _joint2.enablePreprocessing = false;

        _followTarget = controller.gameObject.transform;
    }

    private void Release(InputAction.CallbackContext context)
    {
        if(_joint1 != null)
        {
            Destroy(_joint1);
        }

        if(_joint2 != null)
        {
            Destroy(_joint2);
        }

        if(_grabPoint != null)
        {
            Destroy(_grabPoint.gameObject);
        }

        if(_heldObject != null)
        {
            var targetBody = _heldObject.GetComponent<Rigidbody>();
            targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetBody.interpolation = RigidbodyInterpolation.None;
            _heldObject = null;
        }

        _isGrabbing = false;
        _followTarget = controller.gameObject.transform;
    }
}

