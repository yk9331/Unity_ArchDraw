using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelManager : Singleton<ModelManager> {

    [SerializeField]
    ToolTip toolTip;
    [SerializeField]
    ModelDragItem dragItem;
    [SerializeField]
    Model modelPrefab;


    private bool isShow = false;
    private bool isDrag = false;

    private void Awake() {
        ModelPost.Enter += ModlePost_Enter;
        ModelPost.Exit += ModelPost_Exit;
        ModelPost.BeginDrag += ModelPost_BeginDrag;
        ModelPost.Drag += ModelPost_Drag;
        ModelPost.EndDrag += ModelPost_EndDrag;
        //toolTip.Hide();
    }
    private void Start() {
        toolTip.Hide();
        dragItem.Hide();
    }

    private void Update() {

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.transform as RectTransform, Input.mousePosition, null, out position);

        if (dragItem.gameObject.activeInHierarchy) {
            position += new Vector2(40f, -40f);
            dragItem.SetLocalPosition(position);
        }
        if (toolTip.gameObject.activeInHierarchy) {
            position += new Vector2(0f, 20f);
            toolTip.SetLocalPosition(position);
        }
    }

    void ModlePost_Enter(MyModel model) {
        toolTip.Show();
        toolTip.UpdateToolTip(model.modelName);

    }
    void ModelPost_Exit() {
        toolTip.Hide();
    }

    void ModelPost_BeginDrag(MyModel model) {
        dragItem.Setup(model);
        dragItem.Show();
        toolTip.Hide();
    }

    void ModelPost_Drag() {


    }

    void ModelPost_EndDrag(MyModel model, PointerEventData eventData) {
        dragItem.Hide();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == "Ground") {
                Model parent = Instantiate(modelPrefab, hit.point, Quaternion.identity);
                GameObject mod = Instantiate(model.model, hit.point, Quaternion.identity);
                mod.transform.SetParent(parent.transform);
                parent.Setup();

            }
        }
    }
	

}
