using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelDragItem : MonoBehaviour {


    private MyModel model;

    public void Setup(MyModel model) {
        this.model = model;
        GetComponent<Image>().sprite = model.img;
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


