using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ConvexHullManager : MonoBehaviour{
    public List<Mesh> meshes;
    [SerializeField]
    private List<Vector2> convexHullPoints;

    private MeshFilter _meshFilter;
    private PolygonCollider2D _polygonCollider;
    private GameManager gm;
    private GameObject centroid;
    [SerializeField] private GameObject _prefabCentroid;
    [SerializeField] private GameObject player;
    private void Start(){
        convexHullPoints = new List<Vector2>();
        meshes = new List<Mesh>();
        _meshFilter = GetComponent<MeshFilter>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        gm = GameManager.instance;
    }

    
    public void CreateConvexHull(){
        convexHullPoints = QuickHull.GetConvexHull(gm.tm.GetVector2s());
        Mesh convexHullMesh = CreateMeshFromPolygon(convexHullPoints);
        _meshFilter.mesh = convexHullMesh;
        meshes.Add(convexHullMesh);
    
        if (convexHullPoints.Count > 2) {
            _polygonCollider.points = convexHullPoints.ToArray();
            _polygonCollider.enabled = true;
        }
        //convexHullMesh = CreateUpdatedMesh(convexHullPoints);
    }
/*
    private Mesh CreateUpdatedMesh(List<Vector2> points) {

        float minX = float.MinValue, maxX = float.MaxValue, minY = float.MinValue, maxY = float.MaxValue;
        foreach (Vector2 point in points)
        {  
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }
        float rangeX = maxX - minX;
        float rangeY = maxY - minY;
        // Add points to mesh before generation so that the texture is applied correctly
        for(int i = (int) minX + 1; i < maxX; i++) {
            for(int j = (int) minY + 1; j < maxY; j++) {
                Vector2 toAdd = new Vector2(i, j);
                if (IsPointInsideConvexHull(toAdd))
                    points.Add(toAdd);
            }
        }
        Mesh toReturn = CreateMeshFromPolygon(points);
        Vector2[] uvs = new Vector2[toReturn.vertexCount];
        for (int i = 0; i < toReturn.vertexCount; i++)
        {
            uvs[i] = new Vector2((points[i].x % 1f), (points[i].y % 1f));
        }
        toReturn.uv = uvs;
        return toReturn;
    }*/

    private Mesh CreateMeshFromPolygon(List<Vector2> points)
    {
        Mesh mesh = new Mesh();

        // uvs work but they can't handle that the points are at random locations
        // when you have a point at 13,9 and you give it the uv 1,0.5 and you have another point at 3,4 and you give it uv 0, 0.3
        // the resulting texture will be stretched weirdly
        // we should add more points to the mesh so that we can have uvs that make more sense (in a grid shape)

        Vector3[] vertices = new Vector3[points.Count];
        //Vector2[] uvs = new Vector2[points.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y);
            //uvs[i] = new Vector2((points[i].x % 1f), (points[i].y % 1f));
        }

        List<int> triangles = new List<int>();
        for (int i = 0; i < vertices.Length - 2; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        //mesh.uv = uvs;
        
        CalculateCentroid(vertices);
        
        return mesh;
    }

    public bool IsPointInsideConvexHull(Vector2 point)
    {
        if (convexHullPoints.Count < 3)
        {
            return false;
        }

        return _polygonCollider.OverlapPoint(point);
    }
    
    public GameObject GetConvexHullCentroid(){
        if (centroid != null){
            return centroid;
        }
        return null;
    }

    void CalculateCentroid(Vector3[] vertices){
        if (vertices.Length < 3) return;
        float area = 0f;
        float x = 0f;
        float y = 0f;
        int vertexCount = vertices.Length;

        for (int i = 0; i < vertexCount; i++)
        {
            Vector2 vertex1 = vertices[i];
            Vector2 vertex2 = vertices[(i + 1) % vertexCount];

            float a = vertex1.x * vertex2.y - vertex2.x * vertex1.y;
            area += a;
            x += (vertex1.x + vertex2.x) * a;
            y += (vertex1.y + vertex2.y) * a;
        }

        area *= 0.5f;
        x *= 1 / (6 * area);
        y *= 1 / (6 * area);
        if (centroid != null){
            Destroy(centroid);
        }

        if (x == float.NaN || y == float.NaN) return;
        centroid = Instantiate(_prefabCentroid, transform, true);
        centroid.transform.position = new Vector3(x, y,0);
    }
    
}

