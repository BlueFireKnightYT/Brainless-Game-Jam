using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyPathMover : MonoBehaviour
{
    EnemyBehaviour eB;
    public CreateEnemyPath cEP;
    List<Vector2Int> path;

    private int currentIndex = 0;
    private Vector3 targetWorldPos;
    private void Start()
    {
        eB=GetComponent<EnemyBehaviour>();
        Invoke(nameof(StartPathFinding), 0.01f);   
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0);
    }
    Vector2Int WorldToGrid(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y)
        );
    }

    void StartPathFinding()
    {
        path = cEP.ePF.FindPath(
        WorldToGrid(transform.position),
        WorldToGrid(cEP.player.transform.position)
    );

        if (path == null || path.Count == 0)
            return;

        currentIndex = 0;
        targetWorldPos = GridToWorld(path[currentIndex]);
    }

    void Update()
    {
        if (path == null || path.Count == 0)
            return;

        if (!IsPathValid())
        {
            RecalculatePath();
            return;
        }

        MoveAlongPath();
    }

    void RecalculatePath()
    {
        path = cEP.ePF.FindPath(
            WorldToGrid(transform.position),
            WorldToGrid(cEP.player.transform.position)
        );

        currentIndex = 0;
    }

    void MoveAlongPath()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWorldPos,
            eB.speed * Time.deltaTime
        );

        // reached current node
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.05f)
        {
            currentIndex++;

            if (currentIndex >= path.Count)
            {
                path = null; // done
                return;
            }

            targetWorldPos = GridToWorld(path[currentIndex]);
        }
    }

    bool IsPathValid()
    {
        foreach (var node in path)
        {
            if (GridManager.Instance.IsOccupied(node))
                return false;
        }

        return true;
    }
}
