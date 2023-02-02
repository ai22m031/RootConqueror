using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHullManager : MonoBehaviour
{
    public List<Transform> points;
    private MeshFilter meshFilter;
    private GameManager gm;
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        gm = GameManager.instance;
        createConvexHull();
    }

    public void createConvexHull(){
        List<Vector2> convexHull = ConvexHull.ComputeConvexHull(GetPoints(gm.lm.tm.GetTowers()));
        CreateMesh(convexHull);
    }
    
    private List<Vector2> GetPoints(List<DummyTower> objects)
    {
        List<Vector2> pointList = new List<Vector2>();
        foreach (DummyTower tower in objects)
        {
            pointList.Add(tower.transform.position);
        }
        return pointList;
    }

    private void CreateMesh(List<Vector2> convexHull)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < convexHull.Count; i++)
        {
            vertices.Add(convexHull[i]);
            if (i < convexHull.Count - 2)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}

