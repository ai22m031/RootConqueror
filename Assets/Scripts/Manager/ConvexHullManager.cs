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
    }

    private Mesh CreateMeshFromPolygon(List<Vector2> points)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[points.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y);
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

