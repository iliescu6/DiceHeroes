using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [SerializeField] bool ignoreSortingOrder = false;
    Canvas canvas;
    public Canvas GetCanvas { get => canvas; }

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
}
