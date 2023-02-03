using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleCat : MonoBehaviour
{
    enum State
    {
        None,
        Grounded,
        Flying,
        Wiggling
    }

    public Transform _pointer;

    public float flyingMoveSpeed = 10f;
    public float flyingLerp = 3f;
    public float wiggleMoveSpeed = 1f;
    public float wiggleLerp = 6f;
    public float aimLerp = 1f;


    private float _currentMoveSpeed;

    private State _state;
    private Vector3 _directionVector = Vector3.right;
    private Vector3 _toPointerVector;
    private Vector3 _lerpedPointerVector;


    private bool IsWigglePressed => Input.GetKeyDown(KeyCode.W);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, _toPointerVector * 5f);
        Gizmos.DrawSphere(_toPointerVector, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, _lerpedPointerVector * 5f);
    }

    private void Start()
    {
        Launch();
    }

    private void Launch()
    {
        _state = State.Grounded;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Grounded:
                if (IsWigglePressed)
                {
                    _state = State.Flying;
                }
                break;
            case State.Flying:
                if (IsWigglePressed)
                {
                    _state = State.Wiggling;
                    return;
                }

                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, flyingMoveSpeed, Time.deltaTime * flyingLerp);
                transform.position += _directionVector * _currentMoveSpeed * Time.deltaTime;

                break;
            case State.Wiggling:
                _toPointerVector = GetVector();

                _lerpedPointerVector = Vector3.Lerp(_lerpedPointerVector, _toPointerVector, Time.deltaTime * aimLerp);
                _pointer.LookAt((transform.position + _lerpedPointerVector), Vector3.up);


                if (IsWigglePressed)
                {
                    _directionVector = _toPointerVector;
                    _state = State.Flying;
                    return;
                }

                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, wiggleMoveSpeed, Time.deltaTime * wiggleLerp);
                transform.position += _directionVector * _currentMoveSpeed * Time.deltaTime;

                break;
        }
    }

    private Vector3 GetVector()
    {
        var midPoint = new Vector3(Screen.width, Screen.height, 0f) * 0.5f;
        return Vector3.Normalize(Input.mousePosition - midPoint);
    }
}
