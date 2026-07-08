using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    public List<Vector2Int> DebugOccupiedCells()
    {
        return occupied.ToList();
    }

    public void BakeFromColliders()
    {
        Collider2D[] colliders = FindObjectsByType<Collider2D>();

        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                continue;
            }
            Bounds b = col.bounds;

            int minX = Mathf.RoundToInt(b.min.x);
            int maxX = Mathf.RoundToInt(b.max.x);
            int minY = Mathf.RoundToInt(b.min.y);
            int maxY = Mathf.RoundToInt(b.max.y);
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    occupied.Add(cell);
                }
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        BakeFromColliders();
    }

    public bool IsOccupied(Vector2Int pos)
    {
        return occupied.Contains(pos);
    }

    public void Occupy(Vector2Int pos)
    {
        occupied.Add(pos);
    }

    public void Clear()
    {
        occupied.Clear();
    }

    void OnDrawGizmos()
    {
        if (GridManager.Instance == null) return;

        Gizmos.color = Color.red;

        foreach (var cell in GridManager.Instance.DebugOccupiedCells())
        {
            Vector3 world = new Vector3((cell.x), (cell.y), 0);
            Gizmos.DrawWireCube(world, Vector3.one);
        }
    }
}
