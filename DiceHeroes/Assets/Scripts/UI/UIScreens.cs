using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreens : SingletonTemplate<UIScreens>
{
    public GameScreen[] screens;
    List<GameScreen> screenStack = new List<GameScreen>();

    public static T PushScreen<T>() where T : GameScreen
    {
        T screen = null;

        T screenPrefab = FindScreen<T>(Instance.screens);
        if (screenPrefab != null)
        {
            screen = Instantiate(screenPrefab) as T;
            Instance.screenStack.Add(screen);
        }
        else
        {
            throw new Exception(typeof(T).ToString() + " screen not found");
        }

        return screen;
    }

    public static void PopScreen<T>(T screen) where T: GameScreen 
    {
        Instance.screenStack.Remove(screen);
        if (screen != null)
        {
            Destroy(screen.gameObject);
        }
    }

    //POps the last one
    public static void PopScreen()
    {
        if (Instance.screenStack.Count > 0)
        {
            GameScreen screen = Instance.screenStack[Instance.screenStack.Count - 1];
            if (screen != null)
            {
                Instance.screenStack.Remove(screen);
                Destroy(screen.gameObject);
            }
        }
    }

    static T FindScreen<T>(IEnumerable<GameScreen> array) where T : GameScreen
    {
        GameScreen screen = null;

        foreach (GameScreen gs in array)
        {
            if (gs != null && gs.gameObject.GetComponent<T>() != null)
            {
                screen = gs;
                break;
            }
        }
        return (T)screen;
    }
}
