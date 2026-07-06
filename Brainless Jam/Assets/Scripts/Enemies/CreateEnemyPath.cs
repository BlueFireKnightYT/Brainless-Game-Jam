using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyPath : MonoBehaviour
{
    public EnemyPathfinder ePF;
    public GameObject player;
    public List<Vector2Int> path;
    private void Start()
    {
        Invoke("StartPathFinding", 0.01f);
    }

    void StartPathFinding()
    {
        path = ePF.FindPath(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)), new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y)));

    }

    void OnDrawGizmos()
    {
        if (path == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 worldPos = new Vector3(path[i].x, path[i].y, 0);
            Gizmos.DrawSphere(worldPos, 0.2f);

            if (i < path.Count - 1)
            {
                Vector3 next = new Vector3(path[i + 1].x, path[i + 1].y, 0);
                Gizmos.DrawLine(worldPos, next);
            }
        }
    }
}
