using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ARKitPlaneMeshRender : MonoBehaviour {

    //private static int s_PlaneCount = 0;

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshRenderer meshRenderer;


    private Mesh planeMesh;

    public string planeAnchorIdentifie { get; private set; }

    [SerializeField]
    private Color[] k_PlaneColors = 
        {
            new Color(1.0f, 1.0f, 1.0f),
            new Color(0.956f, 0.262f, 0.211f),
            new Color(0.913f, 0.117f, 0.388f),
            new Color(0.611f, 0.152f, 0.654f),
            new Color(0.403f, 0.227f, 0.717f),
            new Color(0.247f, 0.317f, 0.709f),
            new Color(0.129f, 0.588f, 0.952f),
            new Color(0.011f, 0.662f, 0.956f),
            new Color(0f, 0.737f, 0.831f),
            new Color(0f, 0.588f, 0.533f),
            new Color(0.298f, 0.686f, 0.313f),
            new Color(0.545f, 0.764f, 0.290f),
            new Color(0.803f, 0.862f, 0.223f),
            new Color(1.0f, 0.921f, 0.231f),
            new Color(1.0f, 0.756f, 0.027f)
        };

    public void InitiliazeMesh(ARPlaneAnchor arPlaneAnchor) {
        planeMesh = new Mesh();
        UpdateMesh(arPlaneAnchor);

        meshFilter.mesh = planeMesh;
        planeAnchorIdentifie = arPlaneAnchor.identifier;
        meshRenderer.material.SetColor("_GridColor", k_PlaneColors[0]);
        //m_MeshRenderer.material.SetColor("_GridColor", k_PlaneColors[s_PlaneCount++ % k_PlaneColors.Length]);
        meshRenderer.material.SetFloat("_UvRotation", Random.Range(0.0f, 360.0f));
    }

    public void UpdateMesh(ARPlaneAnchor arPlaneAnchor) {
        if (UnityARSessionNativeInterface.IsARKit_1_5_Supported()) //otherwise we cannot access planeGeometry
        {
            if (arPlaneAnchor.planeGeometry.vertices.Length != planeMesh.vertices.Length ||
                arPlaneAnchor.planeGeometry.textureCoordinates.Length != planeMesh.uv.Length ||
                arPlaneAnchor.planeGeometry.triangleIndices.Length != planeMesh.triangles.Length) {
                planeMesh.Clear();
            }
            //顯示ARKit平面

            //if (GroundPlaneManager.instance.Status == GroundPlaneManagerStatus.GetingARKitGroundPlane || 
            //    arPlaneAnchor.identifier == GroundPlaneManager.instance.groundPlaneAnchorIdentifier) {

            //    if(GroundPlaneManager.instance.Status == GroundPlaneManagerStatus.FinishCreateGroundMesh){

            //    }else{
            //        GetMesh(arPlaneAnchor);
            //    }
            //} 
            //if(GroundPlaneManager.instance.Status ==GroundPlaneManagerStatus.LoadFromSave){
            //    GetMesh(arPlaneAnchor);
            //}

            if (GroundPlaneManager.instance.Status == GroundPlaneManagerStatus.GetingARKitGroundPlane){
                GetMesh(arPlaneAnchor);
            }

            planeMesh.RecalculateBounds();
            planeMesh.RecalculateNormals();

        }

    }

    private void GetMesh(ARPlaneAnchor arPlaneAnchor){
        Vector3 planeNormal = UnityARMatrixOps.GetRotation(arPlaneAnchor.transform) * Vector3.up;
        meshRenderer.material.SetVector("_PlaneNormal", planeNormal);

        planeMesh.vertices = arPlaneAnchor.planeGeometry.vertices;
        planeMesh.uv = arPlaneAnchor.planeGeometry.textureCoordinates;
        planeMesh.triangles = arPlaneAnchor.planeGeometry.triangleIndices;
    }

    void PrintOutMesh() {
        string outputMessage = "\n";
        outputMessage += "Vertices = " + planeMesh.vertices.GetLength(0);
        outputMessage += "\nVertices = [";
        foreach (Vector3 v in planeMesh.vertices) {
            outputMessage += v.ToString();
            outputMessage += ",";
        }
        outputMessage += "]\n Triangles = " + planeMesh.triangles.GetLength(0);
        outputMessage += "\n Triangles = [";
        foreach (int i in planeMesh.triangles) {
            outputMessage += i;
            outputMessage += ",";
        }
        outputMessage += "]\n";
        Debug.Log(outputMessage);

    }

}
