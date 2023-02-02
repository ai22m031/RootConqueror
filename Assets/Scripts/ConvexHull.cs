using System;
using System.Collections.Generic;
using UnityEngine;

public static class ConvexHull{
    public static List<Vector2> ComputeConvexHull(List<Vector2> points){
        if (points == null){
            throw new ArgumentNullException("points");
        }

        if (points.Count < 3){
            throw new ArgumentException("At least 3 points are required to compute a convex hull.");
        }

        List<Vector2> convexHull = new List<Vector2>();

        // Find the bottom-most point
        Vector2 bottomMost = points[0];
        int bottomMostIndex = 0;
        for (int i = 1; i < points.Count; i++){
            if (points[i].y < bottomMost.y || (points[i].y == bottomMost.y && points[i].x < bottomMost.x)){
                bottomMost = points[i];
                bottomMostIndex = i;
            }
        }

        // Swap the bottom-most point with the first point
        points[bottomMostIndex] = points[0];
        points[0] = bottomMost;

        // Sort the remaining points by polar angle
        points.Sort(1, points.Count - 1, new PolarAngleComparer(bottomMost));

        // Use the Graham's scan algorithm to find the convex hull
        convexHull.Add(points[0]);
        convexHull.Add(points[1]);

        for (int i = 2; i < points.Count; i++){
            Vector2 currentPoint = points[i];
            Vector2 top = convexHull[convexHull.Count - 1];
            Vector2 nextToTop = convexHull[convexHull.Count - 2];

            while (convexHull.Count >= 2 && IsCounterClockwise(nextToTop, top, currentPoint)){
                convexHull.RemoveAt(convexHull.Count - 1);
                top = convexHull[convexHull.Count - 1];
                nextToTop = convexHull[convexHull.Count - 2];
            }

            convexHull.Add(currentPoint);
        }

        return convexHull;
    }

    private static bool IsCounterClockwise(Vector2 a, Vector2 b, Vector2 c){
        return (b.x - a.x) * (c.y - a.y) > (b.y - a.y) * (c.x - a.x);
    }

    public class PolarAngleComparer : IComparer<Vector2>
    {
        private Vector2 origin;

        public PolarAngleComparer(Vector2 origin)
        {
            this.origin = origin;
        }

        public int Compare(Vector2 a, Vector2 b)
        {
            float angle1 = Mathf.Atan2(a.y - origin.y, a.x - origin.x);
            float angle2 = Mathf.Atan2(b.y - origin.y, b.x - origin.x);

            if (angle1 < angle2)
            {
                return -1;
            }
            else if (angle1 > angle2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    
}

           
