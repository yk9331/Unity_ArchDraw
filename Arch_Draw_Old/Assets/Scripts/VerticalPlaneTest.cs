using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlaneTest : MonoBehaviour {


    public Transform[] points;
    public GameObject verticalPlanePrefab;
    List<Vector3> mypoint = new List<Vector3>();


    GroundPlane prefab;

    private void Start() {
        mypoint.Clear();
        foreach(Transform t in points){
            mypoint.Add(t.position);
        }
        
        CreateVerticalPlane(mypoint);
    }
    private void CreateVerticalPlane(List<Vector3> pointslist) {
        for (int i = 0; i < pointslist.Count; i++) {
            Vector3 v = (pointslist[(i + 1) % pointslist.Count] - pointslist[i]);
            Vector3 v1 = new Vector3(v.z, 0, -v.x);
            Vector3 center = pointslist[i] + ((pointslist[(i + 1) % pointslist.Count] - pointslist[i]) / 2f) + Vector3.up * 1.5f;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, v1);
            GameObject vPlane = Instantiate(verticalPlanePrefab, center, rotation);
            vPlane.transform.localScale = new Vector3(Vector3.Distance(pointslist[(i + 1) % pointslist.Count], pointslist[i]) * 0.1f,0.3f, 1f);

        }
    }
}
