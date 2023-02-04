using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Keyboard,
    Joystick,
    Mouse
}

public class InputProvider : MonoBehaviour
{
    public ControllerType ControllerType;

    [SerializeField] private KeyCode _wiggleKey= KeyCode.F;
    [SerializeField] private string _wiggleButton = "Accept";
    [SerializeField] private string _horizontal = "Horizontal";
    [SerializeField] private string _vertical = "Vertical";

    public bool IsWiggleReleased => ControllerType switch
    {
        ControllerType.Keyboard => Input.GetKeyUp(_wiggleKey),
        ControllerType.Joystick => Input.GetButtonUp(_wiggleButton),
        ControllerType.Mouse => Input.GetMouseButtonUp(0),
    };

    public bool IsWigglePressed => ControllerType switch
    {
        ControllerType.Keyboard => Input.GetKeyDown(_wiggleKey),
        ControllerType.Joystick => Input.GetButtonDown(_wiggleButton),
        ControllerType.Mouse => Input.GetMouseButtonDown(0),
    };

    public float Horizontal => Input.GetAxis(_horizontal);
    public float Vertical => Input.GetAxis(_vertical);
}
