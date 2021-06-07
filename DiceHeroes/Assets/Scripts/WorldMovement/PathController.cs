using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;
public class PathController : MonoBehaviour
{
    public static PathController Instance;
    public LootTable areaLootTable;
    public List<Node> pathNodes = new List<Node>();

    [SerializeField]
    List<PathPiece> paths = new List<PathPiece>();
    [SerializeField]
    PathPiece currentPath;

    [SerializeField]
    List<PathPiece> prefabPieces;

    [SerializeField]
    PathPiece straightPrefab;

    [SerializeField]
    PathPiece bentPrefab;


    Node[,] grid = new Node[4, 4];
    int length = 15;
    int straigthPieces = 0;
    int x, y;
    int currentRotation = 0;
    int xDirection;
    int yDirection;
    private void Start()
    {
        //CreatePathSecond();
        //CreateGrid();
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //TextAsset file = Resources.Load("LootTables/Area1") as TextAsset;
        //JSON j = JSON.ParseString(file.text);
        //areaLootTable = j.Deserialize<LootTable>();
        //PlayerProfile.Instance.currentLootTable = areaLootTable;
    }

    public void Initialize()
    {
        CreatePathSecond();
        //CreatePathTree();
    }

    //THIS one seems to be the easiest for now
    public void CreatePathSecond()
    {
        pathNodes.Add(new Node(straightPrefab, 0, 0, 0));
        straigthPieces = 2;// Random.Range(3, 6);
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
                pathNodes.Add(new Node(straightPrefab, x, y, currentRotation));
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

                    pathNodes.Add(new Node(bentPrefab, x, y, currentRotation));
                }
                else
                {
                    pathNodes.Add(new Node(bentPrefab, x, y, currentRotation + 90));
                }
                pathNodes[pathNodes.Count - 2].forward = pathNodes[pathNodes.Count - 1];
                length--;
                straigthPieces = 2;// Random.Range(3, 6);
                currentRotation += grades[temp];
                if (currentRotation >= 360)
                {
                    currentRotation -= 360;
                }
            }
        }
        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].piece.PathPieceType == PathPieceType.Straight)
            {
                Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 5, 0, pathNodes[i].y * 10 + 5), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));

            }
            else if (pathNodes[i].piece.PathPieceType == PathPieceType.Bent)
            {
                if (pathNodes[i].rotation == 0 || pathNodes[i].rotation == 360)
                {
                    PathPiece g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 6.25f, 0, pathNodes[i].y * 10 + 6.2f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));
                    g.transform.rotation = Quaternion.Euler(0, pathNodes[i].rotation, 0);
                }
                else if (pathNodes[i].rotation == 90)
                {
                    PathPiece g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 6.25f, 0, pathNodes[i].y * 10 + 3.7f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));
                    g.transform.rotation = Quaternion.Euler(0, pathNodes[i].rotation, 0);
                }
                else if (pathNodes[i].rotation == 180)
                {
                    PathPiece g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 3.75f, 0, pathNodes[i].y * 10 + 3.75f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));
                    g.transform.rotation = Quaternion.Euler(0, pathNodes[i].rotation, 0);
                }
                else if (pathNodes[i].rotation == 270)
                {
                    PathPiece g = Instantiate(pathNodes[i].piece, new Vector3(pathNodes[i].x * 10 + 3.75f, 0, pathNodes[i].y * 10 + 6.25f), Quaternion.Euler(new Vector3(0, pathNodes[i].rotation, 0)));
                    g.transform.rotation = Quaternion.Euler(0, pathNodes[i].rotation, 0);
                }
            }
        }
    }

    void CreatePathTree()
    {
        pathNodes.Add(new Node(paths[0]));
        for (int i = 1; i < paths.Count; i++)
        {
            pathNodes[0].Insert(pathNodes[0], paths[i]);
        }
    }
}
public class Node
{
    public PathPiece piece;
    public Node left, right, forward;
    public Dictionary<string, Node> directions = new Dictionary<string, Node>();
    public bool completed;
    public int x,y;
    public int rotation;
    public Node(PathPiece p,int _x=0,int _y=0,int _rotation=0)
    {
        piece = p;
        piece.x=x = _x;
        piece.y=y = _y;
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

