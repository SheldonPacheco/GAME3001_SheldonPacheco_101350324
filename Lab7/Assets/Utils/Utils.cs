using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static List<Vector2Int> Line(Vector2Int from, Vector2Int to)
        {
            List<Vector2Int> line = new List<Vector2Int>();
            int x = from.x;
            int y = from.y;
            int dx = to.x - from.x;
            int dy = to.y - from.y;
            int sx = dx > 0 ? 1 : -1;
            int sy = dy > 0 ? 1 : -1;
            dx = Mathf.Abs(dx);
            dy = Mathf.Abs(dy);
            int err = (dx > dy ? dx : -dy) / 2;
            while (true)
            {
                line.Add(new Vector2Int(x, y));
                if (x == to.x && y == to.y)
                    break;
                int e2 = err;
                if (e2 > -dx)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dy)
                {
                    err += dx;
                    y += sy;
                }
            }
            return line;
        }
    }
}
