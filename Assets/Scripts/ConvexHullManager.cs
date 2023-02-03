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
        meshes.Add(convexHullMesh);
    }

    private Mesh CreateMeshFromPolygon(List<Vector2> points)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[points.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y, 0);
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

        return mesh;
    }

    
}

