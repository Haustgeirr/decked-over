using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // public Node[] parents;
    public Node[] children;
    public List<Node> history;

    // public GameObject nodeIcon;
    // public Sprite visitedSprited;
    // private Collider _collider;

    private PlayerMovement _playerMovement;
    private SpriteRenderer _nodeIconSpriteRenderer;
    private Sprite _ownSprite;

    public virtual bool OnUse()
    {
        return false;
    }

    public virtual void DisableNode()
    {
        SetHighlight(false);

        if (_playerMovement == null)
            _playerMovement = GameObject.Find("Map").GetComponent<PlayerMovement>();

        _nodeIconSpriteRenderer.sprite = _playerMovement.VisitedNodeSprite;

    }

    public void SetHighlight(bool value)
    {
        if (_playerMovement == null)
            _playerMovement = GameObject.Find("Map").GetComponent<PlayerMovement>();

        if (value)
        {
            // nodeIcon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _nodeIconSpriteRenderer.sprite = _playerMovement.ActiveNodeSprite;
        }
        else
        {

            // nodeIcon.transform.localScale = Vector3.one;
            _nodeIconSpriteRenderer.sprite = _ownSprite;
        }
    }

    public static List<Node> Depthwise(Node start, Node end)
    {

        Stack<Node> work = new Stack<Node>();
        List<Node> visited = new List<Node>();

        // add start node to the work stack and visited list
        work.Push(start);
        visited.Add(start);

        start.history = new List<Node>();

        while (work.Count > 0)
        {
            // get last added item
            Node current = work.Pop();

            // check if its the final node
            if (current == end)
            {
                List<Node> result = current.history;
                result.Add(current);
                return result;
            }
            else
            {
                for (int i = 0; i < current.children.Length; i++)
                {
                    Node currentChild = current.children[i];
                    if (!visited.Contains(currentChild))
                    {
                        work.Push(currentChild);
                        visited.Add(currentChild);
                        currentChild.history = new List<Node>(current.history);
                        currentChild.history.Add(current);
                    }
                }
            }
        }

        return null;

    }

    private void Awake()
    {
        // _collider = GetComponent<Collider>();
        _nodeIconSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _ownSprite = _nodeIconSpriteRenderer.sprite;
    }

    private void OnDrawGizmos()
    {
        if (children.Length > 0)
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            foreach (var child in children)
            {
                if (child != null)
                    Math3D.Arrow2D(this.transform.position, child.transform.position, 2f);
            }
        }
    }
}
