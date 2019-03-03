using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

   
    private Text materialName;
    private void Start() {

    }

    public void UpdateToolTip(string str) {
        materialName = GetComponent<Text>();
        materialName.text = str;
    }

    //顯示
    public void Show() {
        gameObject.SetActive(true);
    }

    //隱藏
    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector2 position) {
        transform.localPosition = position;
    }
}
