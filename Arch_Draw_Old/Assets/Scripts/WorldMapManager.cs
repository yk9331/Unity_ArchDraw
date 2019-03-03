using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;



public class WorldMapManager : Singleton<WorldMapManager> {

    [SerializeField]
    UnityARCameraManager m_ARCameraManager;

    ARWorldMap m_LoadedMap;

    serializableARWorldMap serializedWorldMap;

    List<FilePost> filesPosts = new List<FilePost>();

    [SerializeField]
    InputField nameInput;
    [SerializeField]
    FilePost filePostPrefab;
    [SerializeField]
    RectTransform filePostParent;

    bool isLoading;
    string loadingname;


    void Start() {

        UnityARSessionNativeInterface.ARFrameUpdatedEvent += OnFrameUpdate;
    }

    private void Update() {
        if(isLoading){
            if (m_LastReason == ARTrackingStateReason.ARTrackingStateReasonNone) {
                GroundPlaneManager.instance.LoadFromFile(loadingname);
                isLoading = false;
            }
        }

    }

    ARTrackingStateReason m_LastReason;

    void OnFrameUpdate(UnityARCamera arCamera) {
        if (arCamera.trackingReason != m_LastReason) {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.LogFormat("worldTransform: {0}", arCamera.worldTransform.column3);
            Debug.LogFormat("trackingState: {0} {1}", arCamera.trackingState, arCamera.trackingReason);
            m_LastReason = arCamera.trackingReason;
        }

    }

    static UnityARSessionNativeInterface session {
        get { return UnityARSessionNativeInterface.GetARSessionNativeInterface(); }
    }

    string path {

        get {
            if (nameInput.text != "") {
                string filename = nameInput.text + ".worldmap";
                return Path.Combine(Application.persistentDataPath, "WorldMap", filename);
            }
            return "";
        }
    }

    void OnWorldMap(ARWorldMap worldMap) {
        if (worldMap != null) {
            if (path != "") {
                worldMap.Save(path);
                GroundPlaneManager.instance.Save(nameInput.text);
                Debug.LogFormat("ARWorldMap saved to {0}", path);
                nameInput.text = "";
            }
        }
    }

    public void Save() {
        session.GetCurrentWorldMapAsync(OnWorldMap);

    }

    public void GetFiles() {
        if (filesPosts.Count > 0) {
            foreach (FilePost post in filesPosts) {
                Destroy(post.gameObject);
            }
            filesPosts.Clear();
        }
        string loadpath = Path.Combine(Application.persistentDataPath, "WorldMap");
        if (Directory.Exists(loadpath)) {
            var info = new DirectoryInfo(loadpath);
            var fileInfos = info.GetFiles("*.worldmap");
            foreach (FileInfo file in fileInfos) {
                var post = Instantiate(filePostPrefab, filePostParent);
                post.SetUp(file.Name, file.FullName);
                filesPosts.Add(post);
            }
        } else {
            Directory.CreateDirectory(loadpath);
        }


    }

    public void Load(string name ,string filePath) {

        Debug.LogFormat("Loading ARWorldMap {0}", filePath);

        var worldMap = ARWorldMap.Load(filePath);
        if (worldMap != null) {
            GroundPlaneManager.instance.NewSession(true);

            m_LoadedMap = worldMap;
            Debug.LogFormat("Map loaded. Center: {0} Extent: {1}", worldMap.center, worldMap.extent);
            UnityARSessionNativeInterface.ARSessionShouldAttemptRelocalization = true;

            var config = m_ARCameraManager.sessionConfiguration;
            config.worldMap = worldMap;
            UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking;

            Debug.Log("Restarting session with worldMap");
            session.RunWithConfigAndOptions(config, runOption);

            loadingname = name;
            isLoading = true;
        }
    }


    public void NewSession() {
        GroundPlaneManager.instance.NewSession(false);
        UnityARGeneratePlane.instance.NewSession();
        var config = m_ARCameraManager.sessionConfiguration;
        UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking;
        session.RunWithConfigAndOptions(config, runOption);

    }
}
