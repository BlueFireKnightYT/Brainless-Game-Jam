using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyPathfinder : MonoBehaviour
{
    public GridManager gridManager;
    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public List<Vector2Int> GetNeighbours(Vector2Int pos, HashSet<Vector2Int> extraBlocked = null)
    {
        List<Vector2Int> neighbours = new();

        foreach (var dir in directions)
        {
            Vector2Int next = pos + dir;

            bool blockedByGrid = gridManager.IsOccupied(next);
            bool blockedByTower = extraBlocked != null && extraBlocked.Contains(next);
            if (!blockedByGrid && !blockedByTower)
            {
                neighbours.Add(next);
            }
        }

        return neighbours;
    }

    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public class GridNode
    {
        public Vector2Int position;

        public int gCost;
        public int hCost;

        public GridNode parent;

        public int fCost => gCost + hCost;

        public GridNode(Vector2Int position)
        {
            this.position = position;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target, HashSet<Vector2Int> extraBlocked = null)
{
        List<GridNode> openSet = new();
        HashSet<Vector2Int> closedSet = new();

        GridNode startNode = new GridNode(start);
        openSet.Add(startNode);

        Dictionary<Vector2Int, GridNode> allNodes = new();
        allNodes[start] = startNode;

        while (openSet.Count > 0)
        {
            GridNode current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost ||
                   (openSet[i].fCost == current.fCost &&
                    openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current.position);

            if (current.position == target)
                return RetracePath(current);

            foreach (var neighbourPos in GetNeighbours(current.position, extraBlocked))
            {
                if (closedSet.Contains(neighbourPos))
                    continue;

                int newCost = current.gCost + GetDistance(current.position, neighbourPos);

                GridNode neighbourNode;

                if (!allNodes.TryGetValue(neighbourPos, out neighbourNode))
                {
                    neighbourNode = new GridNode(neighbourPos);
                    allNodes[neighbourPos] = neighbourNode;
                }

                if (newCost < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.gCost = newCost;
                    neighbourNode.hCost = GetDistance(neighbourPos, target);
                    neighbourNode.parent = current;

                    if (!openSet.Contains(neighbourNode))
                        openSet.Add(neighbourNode);
                }
            }
        }
        return null;
    }

    private List<Vector2Int> RetracePath(GridNode endNode)
    {
        List<Vector2Int> path = new();

        GridNode current = endNode;

        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
