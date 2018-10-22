using UnityEngine;

namespace info.shibuya24
{
    /// <summary>
    /// Astarで使用するノードデータ
    /// </summary>
    public struct Node
    {
        /// <summary>
        /// ノードのポジション
        /// </summary>
        internal Vector2Int NodeId { get; }

        /// <summary>
        /// このノードにたどり着く前のノードポジション
        /// </summary>
        internal Vector2Int FromNodeId { get; private set; }

        /// <summary>
        /// 経路として使用できないフラグ
        /// </summary>
        internal bool IsLock { get; private set; }

        /// <summary>
        /// ノードの有無
        /// </summary>
        internal bool IsActive { get; private set; }

        /// <summary>
        /// 必要コスト
        /// </summary>
        internal double MoveCost { get; private set; }

        /// <summary>
        /// ヒューリスティックなコスト
        /// </summary>
        private double _heuristicCost;

        /// <summary>
        /// 空のノードの生成
        /// </summary>
        internal static Node CreateBlankNode(Vector2Int position)
        {
            return new Node(position, new Vector2Int(-1, -1));
        }

        /// <summary>
        /// ノード生成
        /// </summary>
        internal static Node CreateNode(Vector2Int position, Vector2Int goalPosition)
        {
            return new Node(position, goalPosition);
        }

        /// <summary>
        /// CreateBlankNode,CreateNodeを使用してください
        /// </summary>
        internal Node(Vector2Int nodeId, Vector2Int goalNodeId) : this()
        {
            NodeId = nodeId;
            IsLock = false;
            Remove();
            MoveCost = 0;
            UpdateGoalNodeId(goalNodeId);
        }

        /// <summary>
        /// ゴール更新 ヒューリスティックコストの更新
        /// </summary>
        internal void UpdateGoalNodeId(Vector2Int goal)
        {
            // 直線距離をヒューリスティックコストとする
            _heuristicCost = Mathf.Sqrt(
                Mathf.Pow(goal.x - NodeId.x, 2) +
                Mathf.Pow(goal.y - NodeId.y, 2)
            );
        }

        internal double GetScore()
        {
            return MoveCost + _heuristicCost;
        }

        internal void SetFromNodeId(Vector2Int value)
        {
            FromNodeId = value;
        }

        internal void Remove()
        {
            IsActive = false;
        }

        internal void Add()
        {
            IsActive = true;
        }

        internal void SetMoveCost(double cost)
        {
            MoveCost = cost;
        }

        internal void SetIsLock(bool isLock)
        {
            IsLock = isLock;
        }

        internal void Clear()
        {
            Remove();
            MoveCost = 0;
            UpdateGoalNodeId(new Vector2Int(-1, -1));
        }
    }
}
