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
        TextAsset file = Resources.Load("LootTables/Area1") as TextAsset;
        JSON j = JSON.ParseString(file.text);
        areaLootTable = j.Deserialize<LootTable>();
        PlayerProfile.Instance.currentLootTable = areaLootTable;
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
    public Node(PathPiece p)
    {
        piece = p;
        directions.Add("left", null);
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

