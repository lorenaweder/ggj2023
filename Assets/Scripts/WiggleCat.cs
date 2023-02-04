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

    public enum Controller
    {
        Keyboard1,
        Keyboard2,
        Joystick1,
        Joystick2,
        DebugMouse
    }

    public Controller _controller;
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
    private float KeyboardAimAxis => Input.GetAxis("Horizontal");

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

                switch (_controller)
                {
                    case Controller.DebugMouse:
                        _toPointerVector = GetMouseVector();
                        _lerpedPointerVector = Vector3.Lerp(_lerpedPointerVector, _toPointerVector, Time.deltaTime * aimLerp);
                        _pointer.LookAt((transform.position + _lerpedPointerVector), Vector3.up);
                        break;
                    case Controller.Keyboard1:
                    case Controller.Keyboard2:
                        _toPointerVector = GetKeyboardVector();
                        _pointer.LookAt((transform.position + _toPointerVector), Vector3.up);
                        break;
                    case Controller.Joystick1:
                    case Controller.Joystick2:
                        break;
                }

                


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

    public float _mouseAimSpeed = 3f;
    private float _keyboardAimAngle;
    private Vector3 GetKeyboardVector()
    {
        var aimInput = KeyboardAimAxis;
        var current = _keyboardAimAngle;
        var change = aimInput * _mouseAimSpeed * Time.deltaTime;
        _keyboardAimAngle += change;
        if(aimInput > 0 && _keyboardAimAngle > 360f)
        {
            _keyboardAimAngle = (current + change) - 360f;
        }
        if(aimInput <0f && _keyboardAimAngle < 0f)
        {
            _keyboardAimAngle = 360f + (current + change);
        }

        return  Quaternion.Euler(0f, 0f, _keyboardAimAngle) * Vector3.right;
    }

    private Vector3 GetMouseVector()
    {
        var midPoint = new Vector3(Screen.width, Screen.height, 0f) * 0.5f;
        return Vector3.Normalize(Input.mousePosition - midPoint);
    }
}
