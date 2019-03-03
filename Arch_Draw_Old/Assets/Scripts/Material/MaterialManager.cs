using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialManager : Singleton<MaterialManager> {

    [SerializeField]
    ToolTip toolTip;
    [SerializeField]
    MaterialDragItem dragItem;

    private bool isShow = false;
    private bool isDrag = false;

    private void Awake() {
        MaterialPost.Enter += MaterialPost_Enter;
        MaterialPost.Exit += MaterialPost_Exit;
        MaterialPost.BeginDrag += MaterialPost_BeginDrag;
        MaterialPost.Drag += MaterialPost_Drag;
        MaterialPost.EndDrag += MaterialPost_EndDrag;
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

    void MaterialPost_Enter(MyMaterial material) {
        toolTip.Show();
        toolTip.UpdateToolTip(material.materialName);


    }
    void MaterialPost_Exit() {

        toolTip.Hide();
    }

    void MaterialPost_BeginDrag(MyMaterial material) {
        dragItem.Setup(material);
        dragItem.Show();
    }

    void MaterialPost_Drag() {

    }

    void MaterialPost_EndDrag(MyMaterial material, PointerEventData eventData) {

        dragItem.Hide();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Wall") {
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = material.mobileMaterial;
            }
        }
    }
}
