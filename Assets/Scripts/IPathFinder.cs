using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder {
    Stack<Vector2Int> FindPath(Vector2Int from, Vector2Int to, IGridMap map);
}