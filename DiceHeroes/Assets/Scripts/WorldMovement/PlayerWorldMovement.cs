using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldMovement : MonoBehaviour
{
    Node currentPathPiece;
    Node targetPathPiece;

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
        PathController.Instance.Initialize();
        currentPathPiece = PathController.Instance.pathNodes[0];
        SetDirectionButtons();
    }

    public void MoveToPath(Transform target)
    {
        StartCoroutine(MoveToPathCoroutine(target));
        currentPathPiece = targetPathPiece;
        SetDirectionButtons();
    }

    public IEnumerator MoveToPathCoroutine(Transform target)
    {
        player.transform.DOMove(target.position, 1);
        yield return new WaitUntil(()=>Vector3.Distance(target.position,player.transform.position)<0.1f);
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
            leftButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.left.piece.Waypoint); });
            targetPathPiece = currentPathPiece.left;
        }

        if (currentPathPiece.forward == null)
        {
            forwardButton.gameObject.SetActive(false);
        }
        else
        {
            forwardButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.forward.piece.transform); });
            targetPathPiece = currentPathPiece.forward;
        }

        if (currentPathPiece.right == null)
        {
            rightButton.gameObject.SetActive(false);
        }
        else
        {
            rightButton.onClick.AddListener(delegate { MoveToPath(currentPathPiece.right.piece.transform); });
            targetPathPiece = currentPathPiece.right;
        }
    }

    public void DirectionPathButton()
    {
        // currentPathPiece= currentPathPiece.
    }
}
