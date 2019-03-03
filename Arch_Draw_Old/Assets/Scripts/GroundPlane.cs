using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sebastian.Geometry;


public class GroundPlane : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private MeshCollider meshCollider;
  
    public List<Shape> shapes = new List<Shape>();
    public List<GameObject> pointPrefabList = new List<GameObject>();
    public List<GameObject> walls = new List<GameObject>();

    public void UpdateMeshDisplay() {
        CompositeShape compShape = new CompositeShape(shapes);
        Mesh mesh = compShape.GetMesh();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
