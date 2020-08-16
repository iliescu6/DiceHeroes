using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public static PathController Instance;
    public List<Node> pathNodes = new List<Node>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField]
    List<PathPiece> paths = new List<PathPiece>();
    [SerializeField]
    PathPiece currentPath;
    public void Initialize()
    {
        CreatePathTree();
    }

    void CreatePathTree()
    {
        int index = 0;
        pathNodes.Add(new Node(paths[0]));
        for (int i = 1; i < paths.Count; i++)
        {
            if (pathNodes[index].HasUnassignedPath())
            {
                pathNodes[index].Insert(paths[i]);
                pathNodes.Add(new Node(paths[i]));
                if (!pathNodes[index].HasUnassignedPath())
                {
                    index++;
                }
            }
        }
    }

}

public class Node
{
    public PathPiece piece;
    public Node left, right, forward;
    public Dictionary<string, Node> directions = new Dictionary<string, Node>();
    public bool completed;
    public Node(PathPiece p)
    {
        piece = p;
        directions.Add("left", null);
    }

    public void Insert(PathPiece path)
    {
        if (piece.PathPieceType == PathPieceType.Bisection)
        {
            if (this.left == null)
            {
                this.left = new Node(path);
            }
            else if (this.right == null)
            {
                this.right = new Node(path);
                completed = true;
            }
        }
        else if (this.forward == null && piece.PathPieceType == PathPieceType.Straight)
        {
            this.forward = new Node(path);
            this.completed = true;
        }
        else if (piece.PathPieceType == PathPieceType.Intersection)
        {
            if (this.left == null)
            {
                this.left = new Node(path);
            }
            else if (this.right == null)
            {
                this.right = new Node(path);
            }
            else if (this.forward == null)
            {
                this.forward = new Node(path);
                this.completed = true;
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

