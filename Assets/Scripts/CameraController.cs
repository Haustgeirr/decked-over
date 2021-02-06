using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public LayerMask nodeLayer;
    public Node currentNode;
    public Node targetNode;
    public Node hightlightedNode;

    public List<Node> nodePath = new List<Node>();

    public float moveDuration;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private Vector3 _cameraOffset = new Vector3(0, 0, -10f);
    private float _moveTimer;
    // private bool _canSelectTarget = true;

    void Update()
    {
        // RaycastHit hit;
        // Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeLayer))
        // {
        //     var node = hit.transform.gameObject.GetComponent<Node>();

        //     if (node != hightlightedNode)
        //     {
        //         if (hightlightedNode != null)
        //             hightlightedNode.SetHighlight(false);

        //         hightlightedNode = node;
        //         hightlightedNode.SetHighlight(true);
        //     }

        //     if (_canSelectTarget && Input.GetMouseButtonDown(0))
        //     {
        //         if (node != currentNode)
        //         {
        //             targetNode = node;
        //             nodePath = Node.Depthwise(currentNode, targetNode);
        //             StartCoroutine(TraverseNodePath());
        //         }
        //     }
        // }
    }

    private IEnumerator TraverseNodePath()
    {
        // _canSelectTarget = false;

        foreach (var node in nodePath)
        {
            node.DisableNode();
            yield return StartCoroutine(MoveToNode(node));
        }

        nodePath.Clear();
        // _canSelectTarget = true;
        yield return null;
    }

    public void StartMove(Node node)
    {

        StartCoroutine(MoveToNode(node));
    }

    public IEnumerator MoveToNode(Node node)
    {
        _startPosition = transform.position;
        _targetPosition = node.transform.position + _cameraOffset;
        _moveTimer = 0;

        while (_moveTimer < moveDuration)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, Math3D.CubicEaseOut(_moveTimer, moveDuration));
            _moveTimer += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPosition;
        // currentNode = targetNode;
        // targetNode = null;
        yield return null;
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
}
