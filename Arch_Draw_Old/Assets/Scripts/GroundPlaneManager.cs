using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using Sebastian.Geometry;


public enum GroundPlaneManagerStatus {

    GetingARKitGroundPlane,
    CreatingGroundMesh,
    FinishCreateGroundMesh,
    LoadFromSave,
}


public class GroundPlaneManager : Singleton<GroundPlaneManager> {

    [SerializeField]
    PointCloudParticle particle;

    public float maxRayDistance = 30.0f;

    public string groundPlaneAnchorIdentifier { get; set; }

    public GroundPlaneManagerStatus Status;

    [SerializeField] GameObject aimTarget;

    [SerializeField] GroundPlane planePrefab;
    GroundPlane groundPlane;

    public Plane hitTestGroundPlane;

    [SerializeField] GameObject pointPrefab;
    [SerializeField] Material pointPrefabWhiteMat;
    [SerializeField] Material pointPrefabRedMat;

    bool isClose;

    [SerializeField] GameObject verticalPlanePrefab;
    VerticalPlane verticalPlane;

    [SerializeField]
    TextMesh textMeshPrefab;
    TextMesh aimTextMesh;

    List<GameObject> textMesh = new List<GameObject>();
    bool isDisplay = false;

    Vector2 screenCenterPosition {
        get {
            Vector2 position;
            position.x = Screen.width / 2;
            position.y = Screen.height / 2;
            return position;
        }
    }

    Vector3 groundPlanePosition;

    private void Start() {
        aimTarget.SetActive(false);
        Status = GroundPlaneManagerStatus.GetingARKitGroundPlane;
    }

    bool GetGroundPlane(ARPoint point, ARHitTestResultType resultTypes) {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);

        if (hitResults.Count > 0) {
            foreach (var hitResult in hitResults) {
                groundPlaneAnchorIdentifier = hitResult.anchorIdentifier;
                groundPlanePosition = new Vector3(0f, UnityARMatrixOps.GetPosition(hitResult.worldTransform).y, 0f);
                CreateGroundPlane(groundPlanePosition);
                aimTarget.SetActive(true);
                return true;
            }
        }
       
        return false;
    }

    private void CreateGroundPlane(Vector3 position){
        hitTestGroundPlane = new Plane(Vector3.up, position);
        groundPlane = Instantiate(planePrefab, position, Quaternion.identity);
        groundPlane.shapes.Clear();
        groundPlane.shapes.Add(new Shape());
    }

    //private void AimTarget(ARPoint point, ARHitTestResultType resultTypes) {
    //    List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
    //    if (hitResults.Count > 0) {
    //        foreach (var hitResult in hitResults) {
    //            if (hitResult.anchorIdentifier == groundPlaneAnchorIdentifier && hitResult.distance < maxRayDistance) {
    //                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
    //                aimTarget.transform.position = position;
    //                if (groundPlane.shapes[0].points.Count > 0) {
    //                    Vector3 lastPoint = groundPlane.shapes[0].points[groundPlane.shapes[0].points.Count - 1];
    //                    aimTextMesh = UpdateTextMesh(lastPoint, position, aimTextMesh, false);
    //                }
    //                if (groundPlane.pointPrefabList.Count > 1) {
    //                    if (Vector3.Distance(groundPlane.shapes[0].points[0], position) < 0.1f) {
    //                        groundPlane.pointPrefabList[0].GetComponent<MeshRenderer>().material = pointPrefabRedMat;
    //                        isClose = true;
    //                    } else {
    //                        groundPlane.pointPrefabList[0].GetComponent<MeshRenderer>().material = pointPrefabWhiteMat;
    //                        isClose = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private bool AimTargetHitTest(Vector2 position, out Vector3 hitPoint) {
        float enter = 0.0f;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (hitTestGroundPlane.Raycast(ray, out enter)) {
            hitPoint = ray.GetPoint(enter);
            aimTarget.transform.position = hitPoint;
            if (groundPlane.shapes[0].points.Count > 0) {
                Vector3 lastPoint = groundPlane.shapes[0].points[groundPlane.shapes[0].points.Count - 1];
                aimTextMesh = UpdateTextMesh(lastPoint, hitPoint, aimTextMesh, false);
            }
            if (groundPlane.pointPrefabList.Count > 1) {
                if (Vector3.Distance(groundPlane.shapes[0].points[0], hitPoint) < 0.1f) {
                    groundPlane.pointPrefabList[0].GetComponent<MeshRenderer>().material = pointPrefabRedMat;
                    isClose = true;
                } else {
                    groundPlane.pointPrefabList[0].GetComponent<MeshRenderer>().material = pointPrefabWhiteMat;
                    isClose = false;
                }
            }
            return true;
        }
        hitPoint = Vector3.zero;
        return false;
    }

    //private void AddPlanePoint(ARPoint point, ARHitTestResultType resultTypes) {
    //    List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
    //    if (hitResults.Count > 0) {
    //        foreach (var hitResult in hitResults) {
    //            if (hitResult.anchorIdentifier == groundPlaneAnchorIdentifier) {
    //                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
    //                Quaternion rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
    //                if (isClose) {
    //                    foreach (GameObject p in groundPlane.pointPrefabList) {
    //                        p.SetActive(false);
    //                        aimTarget.SetActive(false);
    //                        aimTextMesh.gameObject.SetActive(false);
    //                        Status = GroundPlaneManagerStatus.FinishCreateGroundMesh;
    //                    }
    //                } else {
    //                    groundPlane.pointPrefabList.Add(Instantiate(pointPrefab, position, rotation));
    //                    if (groundPlane.pointPrefabList.Count > 1)
    //                        groundPlane.pointPrefabList[groundPlane.pointPrefabList.Count - 2].GetComponent<MeshRenderer>().material = pointPrefabWhiteMat;
    //                    groundPlane.shapes[0].points.Add(position);
    //                    groundPlane.UpdateMeshDisplay();
    //                }
    //            }
    //        }
    //    }
    //}

    private void AddPlanePointHitTest(Vector3 hitpoint) {
        if (isClose) {
            foreach (GameObject p in groundPlane.pointPrefabList) {
                p.SetActive(false);
                aimTarget.SetActive(false);
                aimTextMesh.gameObject.SetActive(false);
                Status = GroundPlaneManagerStatus.FinishCreateGroundMesh;
            }
        } else {
            groundPlane.pointPrefabList.Add(Instantiate(pointPrefab, hitpoint, Quaternion.identity));
            if (groundPlane.pointPrefabList.Count > 1)
                groundPlane.pointPrefabList[groundPlane.pointPrefabList.Count - 2].GetComponent<MeshRenderer>().material = pointPrefabWhiteMat;
            groundPlane.shapes[0].points.Add(hitpoint);
            groundPlane.UpdateMeshDisplay();
        }
    }

    private void CreateVerticalPlane(List<Vector3> pointslist) {
        for (int i = 0; i < pointslist.Count; i++) {
            Vector3 v = (pointslist[(i + 1) % pointslist.Count] - pointslist[i]);
            Vector3 normal = new Vector3(v.z, 0, -v.x);
            Vector3 center = pointslist[i] + (v / 2f) + Vector3.up * 1.25f;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);
            GameObject vPlane = Instantiate(verticalPlanePrefab, center, rotation);
            groundPlane.walls.Add(vPlane);
            vPlane.transform.localScale = new Vector3(Vector3.Distance(pointslist[(i + 1) % pointslist.Count], pointslist[i]) * 0.1f, 0.25f, 1f);
        }
    }

    public void DisplayDistance(List<Vector3> pointslist) {
        for (int i = 0; i < pointslist.Count; i++) {
            TextMesh text = Instantiate(textMeshPrefab);
            text = UpdateTextMesh(pointslist[i], pointslist[(i + 1) % pointslist.Count], text, true);
            textMesh.Add(text.gameObject);
        }
    }

    public void OnDisplayDistanceBtnClick() {
        if (Status == GroundPlaneManagerStatus.FinishCreateGroundMesh || Status == GroundPlaneManagerStatus.CreatingGroundMesh) {
            if (!isDisplay) {
                isDisplay = true;
                DisplayDistance(groundPlane.shapes[0].points);
            } else {
                isDisplay = false;
                for (int i = 0; i < textMesh.Count; i++) {
                    Destroy(textMesh[i].gameObject);
                }
                textMesh.Clear();
            }
        }
    }

    // Update is called once per frame
    void Update() {

        if (Status == GroundPlaneManagerStatus.GetingARKitGroundPlane) {
            if (Input.touchCount > 0) {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
                    var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                    ARPoint point = new ARPoint {
                        x = screenPosition.x,
                        y = screenPosition.y
                    };

                    if (GetGroundPlane(point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent)) {
                        Status = GroundPlaneManagerStatus.CreatingGroundMesh;
                        particle.gameObject.SetActive(false);
                        return;
                    }
                }
            }
        }

        if (Status == GroundPlaneManagerStatus.CreatingGroundMesh) {
            //ARPoint aimpoint = new ARPoint {
            //    x = screenCenterPosition.x,
            //    y = screenCenterPosition.y
            //};
            //AimTarget(aimpoint, ARHitTestResultType.ARHitTestResultTypeExistingPlane);

            Vector3 hitPoint;
            if (AimTargetHitTest(screenCenterPosition, out hitPoint)) {
                if (Input.touchCount > 0) {
                    var touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began) {
                        //AddPlanePoint(aimpoint, ARHitTestResultType.ARHitTestResultTypeExistingPlane);
                        AddPlanePointHitTest(hitPoint);
                    }
                }
            }
        }

        if (isDisplay) {
            if (textMesh.Count > 0) {
                for (int i = 0; i < textMesh.Count; i++) {
                    //Quaternion rotation = textMesh[i].transform.rotation;
                    //rotation.x = Camera.main.transform.rotation.x;
                    textMesh[i].transform.rotation = Camera.main.transform.rotation;
                }
            }
        }
    }

    public void CreateWallsBtnClick() {
        if (Status == GroundPlaneManagerStatus.FinishCreateGroundMesh && groundPlane.walls.Count == 0)
            CreateVerticalPlane(groundPlane.shapes[0].points);
    }

    public TextMesh UpdateTextMesh(Vector3 point1, Vector3 point2, TextMesh text, bool isCenter) {
        Vector3 center = point1 + ((point2 - point1) / 2f);
        //Vector3 normal = new Vector3(center.z, 0, -center.x);
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);
        //rotation.x = Camera.main.transform.rotation.x;
        float distance = (Vector3.Distance(point1, point2) * 100f);
        if (text == null) {
            text = Instantiate(textMeshPrefab);
        }
        if (isCenter) {
            text.transform.position = center + Vector3.up * 0.1f;
        } else {
            text.transform.position = point2 + Vector3.up * 0.1f;
        }

        text.transform.rotation = Camera.main.transform.rotation;
        string s = System.String.Format("{0:0.00}", distance);
        text.text = "  " + s + " cm ";
        return text;
    }

    public void Save(string filename) {
        string filepath = filename + ".es3";
        ES3.Save<Vector3>("groundPlanePosition", groundPlanePosition, filepath);
        ES3.Save<List<Vector3>>("points", groundPlane.shapes[0].points, filepath);
        ES3.Save<string>("identifier", groundPlaneAnchorIdentifier, filepath);

    }


    public void NewSession(bool isLoading) {
        particle.gameObject.SetActive(true);
        groundPlaneAnchorIdentifier = "";
        groundPlane.shapes[0].points.Clear();
        groundPlane.UpdateMeshDisplay();
        Destroy(groundPlane.gameObject);
        isClose = false;
        if (!isLoading)
            Status = GroundPlaneManagerStatus.GetingARKitGroundPlane;
    }

    public void LoadFromFile(string filename) {
        
        string filepath = filename + ".es3";
        groundPlaneAnchorIdentifier = ES3.Load<string>("identifier", filepath);
        Vector3 position = ES3.Load<Vector3>("groundPlanePosition", filepath);
        CreateGroundPlane(position);
        groundPlane.shapes[0].points = ES3.Load<List<Vector3>>("points", filepath);
        groundPlane.UpdateMeshDisplay();
        isClose = true;
        Status = GroundPlaneManagerStatus.FinishCreateGroundMesh;
    }
}

