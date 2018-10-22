using System.Collections;
using System.Collections.Generic;
using info.shibuya24;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    // 簡易的なシングルトン
    public static RouteManager Instance
    {
        get { return _instance; }
    }

    private static RouteManager _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private AStar _astar;

    public void Initialize(int tileSize)
    {
        _astar = new AStar();
        _astar.Initialize(tileSize);
    }

    public bool SearchRoute(Vector2Int startNodeId, Vector2Int goalNodeId, List<Vector2Int> result)
    {
        return _astar.SearchRoute(startNodeId, goalNodeId, result);
    }
}
