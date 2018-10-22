using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int NodeId { get; private set; }

    public void SetNodeId(Vector2Int nodeId)
    {
        NodeId = nodeId;
    }
}
