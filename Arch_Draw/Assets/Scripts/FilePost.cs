using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilePost : MonoBehaviour {



    string filePath;
    string[] trimName;

    public void SetUp(string nameStr, string filePath) {
        char[] trim = { '.' };
        trimName = nameStr.Split(trim);
        GetComponentInChildren<Text>().text = trimName[0];
        this.filePath = filePath;
    }

    public void OnFileButtonClick() {
        WorldMapManager.instance.Load(trimName[0],filePath);


    }
}
