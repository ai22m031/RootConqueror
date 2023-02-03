using System;
using System.Collections.Generic;
using UnityEngine;

public static class QuickHull
{
    public static List<Vector2> GetConvexHull(List<Vector2> points)
    {
        if (points.Count < 3)
        {
            return points;
        }

        List<Vector2> convexHull = new List<Vector2>();

        // Find the leftmost point
        Vector2 leftmost = points[0];
        int leftmostIndex = 0;
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].x < leftmost.x)
            {
                leftmost = points[i];
                leftmostIndex = i;
            }
        }

        // Start building the hull by moving counter-clockwise
        int p = leftmostIndex, q;
        do
        {
            convexHull.Add(points[p]);

            q = (p + 1) % points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                if (Orientation(points[p], points[i], points[q]) == -1)
                {
                    q = i;
                }
            }

            p = q;
        } while (p != leftmostIndex);

        return convexHull;
    }

    private static int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);

        if (val == 0)
        {
            return 0;
        }
        else
        {
            return (val > 0) ? 1 : -1;
        }
    }
}