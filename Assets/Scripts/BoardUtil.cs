using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardUtil
{
    public static void CreateWall(Vector2 start, Vector2 end, GameObject parent, Material material, float height = 10f)
    {
        var diff = end - start;
        // kupa
        var mid = 1f / 2f * (start + end);

        Vector3[] vertices =
        {
            Vector3.zero,
            new Vector3(diff.x, diff.y, 0), 
            new Vector3(diff.x, diff.y, -height),
            new Vector3(0, 0, -height),
        };

        int[] triangles =
        {
            1, 2, 0,
            2, 3, 0
        };

        var normal = Quaternion.Euler(0, 0, 90) * diff;
        normal = -normal.normalized;

        Vector3[] normals =
        {
            normal,
            normal,
            normal,
            normal
        };
        
        var mesh = new Mesh();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        var wall = new GameObject();
        var meshFilter = wall.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        var meshRenderer = wall.AddComponent<MeshRenderer>();
        wall.transform.position = start;
        meshRenderer.sharedMaterial = material;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        var meshCollider = wall.AddComponent<MeshCollider>();

        wall.transform.parent = parent.transform;

    }

    public static void CreateFloor(Vector2[] verticies, GameObject parent, Material material, PhysicMaterial physicMaterial)
    {
        float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
        foreach (var vertex in verticies)
        {
            minx = (minx > vertex.x) ?  vertex.x : minx;
            miny = (miny > vertex.y) ? vertex.y : miny;

            maxx = (maxx < vertex.x) ? vertex.x : maxx;
            maxy = (maxy < vertex.y) ? vertex.y : maxy;
        }

        Vector3[] quadVerticies =
        {
            new Vector3(minx, miny, 0),
            new Vector3(maxx, miny, 0),
            new Vector3(maxx, maxy, 0),
            new Vector3(minx, maxy, 0), 
        };

        Vector3[] normals =
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back
        };

        int[] tris =
        {
            2, 1, 0,
            3, 2, 0
        };
        
        Mesh mesh = new Mesh();

        mesh.vertices = quadVerticies;
        mesh.normals = normals;
        mesh.triangles = tris;

        var floor = new GameObject();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        var meshFilter = floor.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
        

        var meshRenderer = floor.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        var boxCollider = floor.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0,0, 1);
        boxCollider.size = new Vector3(maxx-minx, maxy-miny, 1);

        floor.transform.parent = parent.transform;
        boxCollider.material = physicMaterial;
    }
}