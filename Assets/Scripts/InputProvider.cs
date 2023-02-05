using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Keyboard,
    Joystick,
    Mouse,
    Any
}

public class InputProvider : MonoBehaviour
{
    public ControllerType ControllerType;

    [SerializeField] private KeyCode _wiggleKey = KeyCode.F;
    [SerializeField] private string _wiggleButton = "Accept";
    [SerializeField] private string _horizontal = "Horizontal";
    [SerializeField] private string _vertical = "Vertical";
    [SerializeField] private KeyCode _meowKey = KeyCode.M;
    [SerializeField] private string _meowButton = "Meow";

    public bool IsWiggleReleased => ControllerType switch
    {
        ControllerType.Keyboard => Input.GetKeyUp(_wiggleKey),
        ControllerType.Joystick => Input.GetButtonUp(_wiggleButton),
        ControllerType.Mouse => Input.GetMouseButtonUp(0),
        ControllerType.Any => Input.GetMouseButtonUp(0) || Input.GetButtonUp(_wiggleButton),
    };

    public bool IsWigglePressed => ControllerType switch
    {
        ControllerType.Keyboard => Input.GetKeyDown(_wiggleKey),
        ControllerType.Joystick => Input.GetButtonDown(_wiggleButton),
        ControllerType.Mouse => Input.GetMouseButtonDown(0),
        ControllerType.Any => Input.GetMouseButtonDown(0) || Input.GetButtonDown(_wiggleButton),
    };

    public bool IsMeowPressed => ControllerType switch
    {
        ControllerType.Joystick => Input.GetButtonDown(_meowButton),
        ControllerType.Keyboard => Input.GetKeyDown(_meowKey),
        ControllerType.Mouse => Input.GetKeyDown(_meowKey),
        ControllerType.Any => Input.GetKeyDown(_meowKey) || Input.GetButtonDown(_meowButton),
    };

    public float Horizontal => Input.GetAxis(_horizontal);
    public float Vertical => Input.GetAxis(_vertical);

    public bool IsWiggleMouseHeld => Input.GetMouseButton(0);
}
