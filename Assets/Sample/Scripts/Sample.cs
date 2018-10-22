using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sample : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _unityChanMoveTransform;

    [SerializeField]
    private Transform _unityChanRotateTransform;

    [SerializeField]
    private Animator _unitychanAnimator;

    public int tileSize = 15;

    public Tile tilePrefab;
    private Tile[,] tiles;

    private Vector2Int _currentNodeId;
    public float walkSpeed = 0.6f;
    List<Vector2Int> _routeList = new List<Vector2Int>();

    [SerializeField]
    private Material _pattern;

    void Start()
    {
        tiles = new Tile[tileSize, tileSize];
        // Astar初期化
        RouteManager.Instance.Initialize(tileSize);

        int count = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                var tile = Instantiate(tilePrefab) as Tile;
                tiles[x, y] = tile;
                tile.SetNodeId(new Vector2Int(x, y));
                if (count++ % 2 == 0)
                {
                    tile.GetComponent<Renderer>().material = _pattern;
                }

                tile.transform.localPosition = new Vector3(x, tile.transform.localPosition.y, y);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                var hitObject = hit.collider.gameObject;
                var tile = hitObject.GetComponent<Tile>();
                Goto(tile.NodeId);
            }
        }
    }

    void Goto(Vector2Int goalNodeId)
    {
        if (RouteManager.Instance.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // 移動
            StartMove();
        }

        _currentNodeId = goalNodeId;
    }

    private Coroutine _moveCoroutine;
    private Tween _moveTween;

    private void StartMove()
    {
        _moveTween?.Kill();
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(_StartMove());
    }

    IEnumerator _StartMove()
    {
        _unitychanAnimator.CrossFade("Running(loop)", 0);
        var wait = new WaitForSeconds(walkSpeed);
        for (int i = 0; i < _routeList.Count; i++)
        {
            var nodeId = _routeList[i];
            var goal = new Vector3(nodeId.x, 0, nodeId.y);
            _unityChanRotateTransform.localRotation =
                Quaternion.LookRotation(goal - _unityChanMoveTransform.localPosition);

            _moveTween = _unityChanMoveTransform.DOLocalMove(goal, walkSpeed)
                .SetEase(Ease.Linear);
            yield return wait;
        }

        _moveCoroutine = null;

        _unitychanAnimator.CrossFade("Standing(loop)", 0);
    }
}
