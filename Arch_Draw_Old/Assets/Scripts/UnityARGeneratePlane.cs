using System;
using System.Collections.Generic;
using Collections.Hybrid.Generic;

namespace UnityEngine.XR.iOS {
    public class UnityARGeneratePlane : Singleton<UnityARGeneratePlane> {
        public GameObject planePrefab;
        public UnityARAnchorManager unityARAnchorManager;

        // Use this for initialization
        void Start() {
            unityARAnchorManager = new UnityARAnchorManager();
            UnityARUtility.InitializePlanePrefab(planePrefab);
        }

        void OnDestroy() {
            unityARAnchorManager.Destroy();
        }

        public void NewSession() {
            unityARAnchorManager.Destroy();
            unityARAnchorManager = new UnityARAnchorManager();
            UnityARUtility.InitializePlanePrefab(planePrefab);
        }


    }
}

