using UnityEngine;

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
