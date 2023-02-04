using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Keyboard,
    Joystick,
    DebugMouse
}

public class InputProvider : MonoBehaviour
{
    public ControllerType ControllerType;

    [SerializeField] private KeyCode _wiggleKey= KeyCode.F;
    [SerializeField] private string _horizontal = "Horizontal";
    [SerializeField] private string _vertical = "Vertical";

    public bool IsWigglePressed => Input.GetKeyDown(_wiggleKey);
    public float Horizontal => Input.GetAxis(_horizontal);
    public float Vertical => Input.GetAxis(_vertical);
}
