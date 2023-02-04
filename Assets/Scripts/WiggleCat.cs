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

    public InputProvider _inputProvider;
    public Transform _pointer;

    public float flyingMoveSpeed = 10f;
    public float flyingLerp = 3f;
    public float wiggleMoveSpeed = 1f;
    public float wiggleLerp = 6f;
    public float aimLerp = 1f;
    public float mouseAimLerp = 10f;


    private float _currentMoveSpeed;

    private State _state;
    private float _keyboardAimAngle;
    private Vector3 _directionVector = Vector3.right;
    private Vector3 _toPointerVector;
    private Vector3 _lerpedPointerVector;


    private bool IsWigglePressed => _inputProvider.IsWigglePressed;
    private float KeyboardAimAxisH => _inputProvider.Horizontal;
    private float KeyboardAimAxisV => _inputProvider.Vertical;


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

                switch (_inputProvider.ControllerType)
                {
                    case ControllerType.DebugMouse:
                        _toPointerVector = GetMouseVector();
                        _lerpedPointerVector = Vector3.Lerp(_lerpedPointerVector, _toPointerVector, Time.deltaTime * aimLerp);
                        _pointer.LookAt((transform.position + _lerpedPointerVector), Vector3.up);
                        break;
                    case ControllerType.Keyboard:
                    case ControllerType.Joystick:
                        _toPointerVector = GetKeyboardVector();
                        _pointer.LookAt((transform.position + _toPointerVector), Vector3.up);
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

    private Vector3 GetKeyboardVector()
    {
        if (Mathf.Approximately(KeyboardAimAxisV, 0f) && Mathf.Approximately(KeyboardAimAxisH, 0f)) return _toPointerVector;

        var aimInput = Mathf.Atan2(KeyboardAimAxisV, KeyboardAimAxisH);
        _keyboardAimAngle = LerpAngle(_keyboardAimAngle, aimInput, Time.deltaTime * mouseAimLerp);

        return  Quaternion.Euler(0f, 0f, _keyboardAimAngle * Mathf.Rad2Deg) * Vector3.right;
    }

    private Vector3 GetMouseVector()
    {
        var midPoint = new Vector3(Screen.width, Screen.height, 0f) * 0.5f;
        return Vector3.Normalize(Input.mousePosition - midPoint);
    }

    float LerpAngle(float from, float to, float t)
    {
        return from + ShortestAngle(from, to) * t;
    }

    float ShortestAngle(float from, float to)
    {
        var max = Mathf.PI * 2;
        var diff = Modulo(to - from, max);
        return Modulo(2f * diff, max) - diff;
    }

    float Modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}
