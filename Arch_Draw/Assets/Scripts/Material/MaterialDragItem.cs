using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialDragItem : MonoBehaviour {

    private MyMaterial material;

    public void Setup(MyMaterial material) {
        this.material = material;
        GetComponent<Image>().sprite = material.img;
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
