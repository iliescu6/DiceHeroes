using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : SingletonTemplate<InputController>
{
    public static IPlatformInput platformInput;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        platformInput = new PCInput();
#endif
    }

    //Return value while it's pressed
    public static bool IsButtonPressed()
    {
        return platformInput.IsInputPressed();
    }
    //Return only in frame it was touched
    public static bool IsButtonDown()
    {
        return platformInput.IsInputDown();
    }
    //Returns only in frame it was touched
    public static bool IsButtonUp()
    {
        return platformInput.IsInputUp();
    }

    public static bool IsOverUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1);
#endif
    }

    public static GameObject GetPressedObject()
    {
        return EventSystem.current.currentSelectedGameObject;
    }
}
