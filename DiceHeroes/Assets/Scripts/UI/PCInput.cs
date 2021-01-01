using UnityEngine;

public class PCInput : IPlatformInput
{
    public bool IsInputPressed()
    {
        return Input.GetMouseButton(0);
    }

    public bool IsInputUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool IsInputDown()
    {

        return Input.GetMouseButtonDown(0);
    }
}
