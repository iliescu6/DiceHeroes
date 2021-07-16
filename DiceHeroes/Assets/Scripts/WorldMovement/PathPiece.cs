using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, South, West, East }
public enum PathEventType { None, Combat, FreeChest, Final }
public enum PathPieceType { Straight, Bisection, Intersection, Bent, Final }

public class PathPiece : MonoBehaviour
{
    [SerializeField]
    List<Direction> availableDirections = new List<Direction>();
    [SerializeField]
    PathPieceType pathPieceType;
    List<CharacterStats> enemies = new List<CharacterStats>();
    [SerializeField]
    Transform waypoint;
    [SerializeField]
    PathEventType pathEventType;
    [SerializeField]
    public GameObject chest;
    public int rotation;
    public int x, y;

    public Transform Waypoint
    {
        get { return waypoint; }
    }

    public PathEventType PathEventType
    {
        get { return pathEventType; }
        set { pathEventType = value; }
    }

    public PathPieceType PathPieceType
    {
        get { return pathPieceType; }
        set { pathPieceType = value; }
    }

    public void Initialize(List<CharacterStats> enemies)
    {
        if (pathEventType == PathEventType.Combat && enemies!=null)
        { 
        
        }
    }

    public void ShowEvent()
    {

    }
}
