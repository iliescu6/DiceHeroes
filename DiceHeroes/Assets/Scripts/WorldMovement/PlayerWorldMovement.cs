using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldMovement : MonoBehaviour
{
    Node currentPathPiece;
    Node targetPathPiece;
    PlayerProfile playerProfile;
    [SerializeField]
    PathController pathController;
    [SerializeField]
    Transform player;
    [SerializeField]
    Button leftButton;
    [SerializeField]
    Button forwardButton;
    [SerializeField]
    Button rightButton;
    private void Start()
    {
        //if (pathController.pathNodes.Count == 0)
        //{
        //    pathController.CreatePathSecond();
        //}

        playerProfile = PlayerProfile.Instance;
        //if (playerProfile.CurrentMap != null)
        //{
        //    currentPathPiece = playerProfile.CurrentMap;
        //}
        //else
        //{
        pathController.Initialize();
        currentPathPiece = pathController.pathNodes[0];
        // }
        SetDirectionButtons();
    }

    public void MoveToPath(Transform target)
    {
        StartCoroutine(MoveToPathCoroutine(target));
        currentPathPiece = targetPathPiece;
        playerProfile.CurrentMap = currentPathPiece;
        if (currentPathPiece.piece.PathEventType != PathEventType.None)
            WorldPanelsController.Instance.ShowPathEventPanel(currentPathPiece.piece);
        SetDirectionButtons();
    }

    public IEnumerator MoveToPathCoroutine(Transform target)
    {
        player.transform.DOMove(target.position, 1);
        Vector3 vec = target.TransformPoint(target.position);
        yield return new WaitUntil(() => Vector3.Distance(target.position, player.transform.position) < 0.1f);
    }

    void SetDirectionButtons()
    {
        leftButton.onClick.RemoveAllListeners();
        forwardButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        if (currentPathPiece.left == null)
        {
            leftButton.gameObject.SetActive(false);
        }
        else
        {
            leftButton.gameObject.SetActive(true);
            leftButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.left.piece.Waypoint); });
            targetPathPiece = currentPathPiece.left;
        }

        if (currentPathPiece.forward == null)
        {
            forwardButton.gameObject.SetActive(false);
        }
        else
        {
            forwardButton.gameObject.SetActive(true);
            forwardButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.forward.piece.Waypoint); });
            targetPathPiece = currentPathPiece.forward;
        }

        if (currentPathPiece.right == null)
        {
            rightButton.gameObject.SetActive(false);
        }
        else
        {
            rightButton.gameObject.SetActive(true);
            rightButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.right.piece.Waypoint); });
            targetPathPiece = currentPathPiece.right;
        }
    }

    public void DirectionPathButton()
    {
        // currentPathPiece= currentPathPiece.
    }
}
