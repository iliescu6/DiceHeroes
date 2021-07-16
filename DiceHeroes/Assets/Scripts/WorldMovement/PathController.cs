using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;
public class PathController : MonoBehaviour
{
    public LootTable areaLootTable;
    public List<Node> pathNodes = new List<Node>();

    [SerializeField]
    PathPiece currentPath;

    [SerializeField]
    List<PathPiece> prefabPieces;

    [SerializeField]
    PathPiece straightPrefab;

    [SerializeField]
    PathPiece pathEndingPrefab;

    [SerializeField]
    PathPiece bentPrefab;

    [SerializeField]
    int minDistanceBetweenEvents=2;

    [SerializeField]
    int maxDistanceBetweenEvents=3;

    [SerializeField]
    Transform levelContainer;

    int distanceBetweenEvents;

    int length = 15;
    int straigthPieces = 0;
    int x, y;
    int currentRotation = 0;
    private void Start()
    {
    }

    public void Initialize()
    {
        CreatePathSecond();
    }

    //THIS one seems to be the easiest for now
    public void CreatePathSecond()
    {
        pathNodes.Add(new Node(straightPrefab, 0, 0, 0));
        distanceBetweenEvents = Random.Range(minDistanceBetweenEvents, maxDistanceBetweenEvents + 1);
        PathEventType pathEventType = PathEventType.None;
        straigthPieces = 2;
        while (length != 0)
        {
            if (currentRotation == 270)
            {
                y--;
            }
            else if (currentRotation == 0)
            {
                x--;
            }
            else if (currentRotation == 180)
            {
                x++;
            }
            else if (currentRotation == 90)
            {
                y++;
            }
            if (straigthPieces > 0)
            {
                SetEventType(ref pathEventType);
                pathNodes.Add(new Node(straightPrefab, x, y, currentRotation, pathEventType));
                pathNodes[pathNodes.Count - 2].forward = pathNodes[pathNodes.Count - 1];
                straigthPieces--;
                length--;
            }
            else if (straigthPieces <= 0)
            {
                int[] grades = { 90, 270 };
                int temp = Random.Range(0, grades.Length);
                if (temp == 0)
                {
                    SetEventType(ref pathEventType);
                    pathNodes.Add(new Node(bentPrefab, x, y, currentRotation, pathEventType));
                }
                else
                {
                    SetEventType(ref pathEventType);
                    pathNodes.Add(new Node(bentPrefab, x, y, currentRotation + 90, pathEventType));
                }
                pathNodes[pathNodes.Count - 2].forward = pathNodes[pathNodes.Count - 1];
                length--;
                straigthPieces = Random.Range(3, 6);
                currentRotation += grades[temp];
                if (currentRotation >= 360)
                {
                    currentRotation -= 360;
                }
            }
        }

        if (length == 0)
        {
            if (currentRotation == 270)
            {
                y--;
            }
            else if (currentRotation == 0)
            {
                x--;
            }
            else if (currentRotation == 180)
            {
                x++;
            }
            else if (currentRotation == 90)
            {
                y++;
            }

            pathEventType = PathEventType.Final;
            pathNodes.Add(new Node(pathEndingPrefab, x, y, currentRotation, pathEventType));
            pathNodes[pathNodes.Count - 2].forward = pathNodes[pathNodes.Count - 1];
        }

        for (int i = 0; i < pathNodes.Count; i++)
        {
            PathPiece g=null;
            //Shitty modular prefabs....
            if (pathNodes[i].piece.PathPieceType == PathPieceType.Straight)
            {
                g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 5, 0, pathNodes[i].y * 10 + 5), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));
            }
            else if (pathNodes[i].piece.PathPieceType == PathPieceType.Bent)
            {
                if (pathNodes[i].rotation == 0 || pathNodes[i].rotation == 360)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 6.25f, 0, pathNodes[i].y * 10 + 6.2f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 90)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 6.25f, 0, pathNodes[i].y * 10 + 3.7f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 180)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 3.75f, 0, pathNodes[i].y * 10 + 3.75f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 270)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 3.75f, 0, pathNodes[i].y * 10 + 6.25f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
            }
            else if (pathNodes[i].piece.PathPieceType == PathPieceType.Final)
            {
                if (pathNodes[i].rotation == 0 || pathNodes[i].rotation == 360)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 10f, 0, pathNodes[i].y * 10 + 5), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 90)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 5, 0, pathNodes[i].y * 10 ), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 180)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10, 0, pathNodes[i].y * 10 +5f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
                else if (pathNodes[i].rotation == 270)
                {
                    g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 5f, 0, pathNodes[i].y * 10 +10), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

                }
            }
            g.PathEventType = pathNodes[i].eventType;
            g.transform.parent = levelContainer;
            pathNodes[i].piece = g;//I know something is fucked up somewhere but am tired and running on low caffeine...or already ran out of it
            if (g.PathEventType == PathEventType.FreeChest)
            {
                g.chest.gameObject.SetActive(true);
            }
        }
    }

    void SetEventType(ref PathEventType type)
    {
        if (distanceBetweenEvents <= 0)
        {
            type = BasicChanceRoll();
            distanceBetweenEvents = Random.Range(minDistanceBetweenEvents, maxDistanceBetweenEvents + 1);
        }
        else
        {
            type = PathEventType.None;
            distanceBetweenEvents--;
        }
    }

    PathEventType BasicChanceRoll()
    {
        PathEventType type;
        int chance = Random.Range(0, 6);
        if (chance > 2)
        {
            type = PathEventType.Combat;
        }
        else
        {
            type = PathEventType.FreeChest;
        }
        return type;
    }
}
public class Node
{
    public PathPiece piece;
    public Node left, right, forward;
    public Dictionary<string, Node> directions = new Dictionary<string, Node>();
    public bool completed;
    public int x, y;
    public int rotation;
    public PathEventType eventType;
    public Node(PathPiece p, int _x = 0, int _y = 0, int _rotation = 0, PathEventType _eventType = PathEventType.None)
    {
        eventType = _eventType;
        piece = p;
        piece.PathEventType = _eventType;
        piece.x = x = _x;
        piece.y = y = _y;
        rotation = _rotation;
    }

    void SimpleInser(Node temp, PathPiece path)
    {
        if (temp.piece.PathPieceType == PathPieceType.Straight || temp.piece.PathPieceType == PathPieceType.Bent)
        {
            temp.forward = new Node(path);
        }
    }

    public void Insert(Node temp, PathPiece path)
    {
        Queue<Node> q = new Queue<Node>();
        q.Enqueue(temp);
        while (q.Count != 0)
        {
            temp = q.Peek();
            q.Dequeue();
            if (temp.piece.PathPieceType == PathPieceType.Bisection)
            {
                if (temp.left == null)
                {
                    temp.left = new Node(path);
                    break;
                }
                else
                {
                    q.Enqueue(temp.left);
                }
                if (temp.right == null)
                {
                    temp.right = new Node(path);
                    break;
                }
                else
                {
                    q.Enqueue(temp.right);
                }
            }
            else if (temp.piece.PathPieceType == PathPieceType.Straight)
            {
                if (temp.forward == null)
                {
                    temp.forward = new Node(path);
                    break;
                }
                else
                {
                    q.Enqueue(temp.forward);
                }
            }
            else if (temp.piece.PathPieceType == PathPieceType.Intersection)
            {
                if (temp.left == null)
                {
                    temp.left = new Node(path);
                }
                else if (temp.right == null)
                {
                    temp.right = new Node(path);
                }
                else if (temp.forward == null)
                {
                    temp.forward = new Node(path);
                    temp.completed = true;
                }
            }
        }
    }

    public bool HasUnassignedPath()
    {
        bool unasingedPath = false;
        PathPieceType type = this.piece.PathPieceType;
        if (type == PathPieceType.Straight)
        {
            if (this.forward == null)
            {
                unasingedPath = true;
            }
        }
        else if (type == PathPieceType.Bisection)
        {
            if (this.left == null || this.right == null)
            {
                unasingedPath = true;
            }
        }
        else if (type == PathPieceType.Intersection)
        {
            if (this.left == null || this.forward == null || this.right == null)
            {
                unasingedPath = true;
            }
        }
        return unasingedPath;
    }
}

