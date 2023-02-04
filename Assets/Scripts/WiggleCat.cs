using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CatHitInfo
{
    public Collision Collision;
    public bool IsDangerousHit;
}

public class WiggleCat : MonoBehaviour
{
    enum State
    {
        None,
        Grounded,
        Flying,
        Wiggling,
        Stunned
    }

    public bool _allowAimFlying;
    public InputProvider _inputProvider;
    public Transform _pointer;

    [Header("Flying")]
    public float flyingMoveSpeed = 10f;
    public float flyingLerp = 3f;

    [Header("Wiggle")]
    public float wiggleMoveSpeed = 1f;
    public float wiggleLerp = 6f;

    [Header("Stunned")]
    public float stunnedMoveSpeed = 10f;
    public float stunnedLerp = 3f;
    public float stunnedAngleSpeed = 10;
    public float stunnedDuration = 2f;

    [Header("Aim")]
    public float aimLerp = 1f;
    public float mouseAimLerp = 10f;


    private float _currentMoveSpeed;

    private State _state;
    private float _stunnedTime;
    private float _keyboardAimAngle;
    private Vector3 _directionVector = Vector3.right;
    private Vector3 _toPointerVector;
    private Vector3 _lerpedPointerVector;

    private Rigidbody _rb;

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
        _rb = GetComponent<Rigidbody>();
        Launch();
    }

    private void Launch()
    {
        _state = State.Grounded;
    }

    public void TakeHit(CatHitInfo hit)
    {
        var normal = hit.Collision.contacts[0].normal;
        var clamped2dCollision = Vector3.ProjectOnPlane(normal, Vector3.forward);
        //Debug.Log($"Collision {normal}, projected {clamped2dCollision}");

        _directionVector = Vector3.Reflect(_directionVector, clamped2dCollision);
        if (hit.IsDangerousHit)
        {
            _state = State.Stunned;
            _stunnedTime = Time.time;
        }
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

                if (_allowAimFlying)
                {
                    Aim();
                }
                break;
            case State.Wiggling:

                Aim();

                if (_inputProvider.IsWiggleReleased)
                {
                    _directionVector = _toPointerVector;
                    _state = State.Flying;
                    return;
                }

                break;
            case State.Stunned:
                if (Time.time - _stunnedTime > stunnedDuration)
                {
                    _state = State.Flying;
                    return;
                }

                _keyboardAimAngle = LerpAngle(_keyboardAimAngle, (_keyboardAimAngle + stunnedAngleSpeed) % (Mathf.PI * 2), Time.deltaTime * mouseAimLerp);
                _toPointerVector = Quaternion.Euler(0f, 0f, _keyboardAimAngle * Mathf.Rad2Deg) * Vector3.right;
                _pointer.LookAt((transform.position + _toPointerVector), Vector3.up);
                break;
        }
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case State.Grounded:
                break;
            case State.Flying:
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, flyingMoveSpeed, Time.deltaTime * flyingLerp);
                _rb.position += _directionVector * _currentMoveSpeed * Time.deltaTime;

                break;
            case State.Wiggling:
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, wiggleMoveSpeed, Time.deltaTime * wiggleLerp);
                _rb.position += _directionVector * _currentMoveSpeed * Time.deltaTime;
                break;
            case State.Stunned:
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, stunnedMoveSpeed, Time.deltaTime * stunnedLerp);
                _rb.position += _directionVector * _currentMoveSpeed * Time.deltaTime;
                break;
        }
    }

    private void Aim()
    {
        switch (_inputProvider.ControllerType)
        {
            case ControllerType.Mouse:
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
        var pointing = Vector3.Normalize(Input.mousePosition - midPoint);

        var aimInput = Mathf.Atan2(pointing.y, pointing.x);
        _keyboardAimAngle = LerpAngle(_keyboardAimAngle, aimInput, Time.deltaTime * mouseAimLerp);

        return Quaternion.Euler(0f, 0f, _keyboardAimAngle * Mathf.Rad2Deg) * Vector3.right;
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
