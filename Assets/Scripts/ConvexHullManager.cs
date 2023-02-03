using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHullManager : MonoBehaviour{
    public List<Mesh> meshes;
    [SerializeField]
    private List<Vector2> convexHullPoints;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private GameManager gm;
    private GameObject centroid;
    [SerializeField] private GameObject _prefabCentroid;
    [SerializeField] private GameObject player;
    private void Start(){
        convexHullPoints = new List<Vector2>();
        meshes = new List<Mesh>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        gm = GameManager.instance;
    }

    
    public void CreateConvexHull(){
        convexHullPoints = QuickHull.GetConvexHull(gm.lm.tm.GetVector2s());
        Mesh convexHullMesh = CreateMeshFromPolygon(convexHullPoints);
        _meshFilter.mesh = convexHullMesh;
        if (convexHullMesh.vertices.Length > 3){
            _meshCollider.sharedMesh = convexHullMesh;
        }
        
        meshes.Add(convexHullMesh);
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
        if (vertices.Length >= 3){
            Vector2 centroidPos = CalculateCentroid(vertices);
            centroid =  Instantiate(_prefabCentroid, transform, false);
            centroid.transform.position = centroidPos;
        }
        return mesh;
    }

    Vector2 CalculateCentroid(Vector3[] vertices){
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
        return new Vector2(x, y);
    }

    public GameObject GetConvexHullCentroid(){
        if (centroid != null){
            return centroid;
        }
        return null;
    }

    public bool IsPlayerInsideHull(){
        return _meshCollider.bounds.Contains(player.transform.position);
    }
    
}

