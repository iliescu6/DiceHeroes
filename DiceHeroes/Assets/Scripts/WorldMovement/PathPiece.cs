using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, South, West, East }
public enum PathEventType { None, Combat, FreeChest, Final }
public enum PathPieceType { Straight, Bisection, Intersection, Deadend }

public class PathPiece : MonoBehaviour
{
    [SerializeField]
    List<Direction> availableDirections = new List<Direction>();
    [SerializeField]
    PathPieceType pathPieceType;
    Dictionary<Direction, PathPiece> directions = new Dictionary<Direction, PathPiece>();
    [SerializeField]
    Transform waypoint;
    [SerializeField]
    PathEventType pathEventType;

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

    public Dictionary<Direction, PathPiece> Directions
    {
        get { return directions; }
        set { directions = value; }
    }

    public void Initialize(List<Direction> directionList)
    {
        foreach (Direction direction in directionList)
        {
            availableDirections.Add(direction);
        }
    }

    public void ShowEvent()
    {

    }
}
