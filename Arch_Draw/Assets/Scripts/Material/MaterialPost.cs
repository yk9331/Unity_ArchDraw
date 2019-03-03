using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MaterialPost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public MyMaterial material { get; private set; }
    private ScrollRect parentScrollRect;

    public void Setup(MyMaterial material, ScrollRect parent) {
        this.material = material;
        GetComponent<Image>().sprite = material.img;
        parentScrollRect = parent;
    }



    public static Action<MyMaterial> Enter;
    public static Action Exit;
    public static Action<MyMaterial> BeginDrag;
    public static Action Drag;
    public static Action<MyMaterial, PointerEventData> EndDrag;

    public void OnPointerEnter(PointerEventData eventData) {
        Enter(material);
    }

    public void OnPointerExit(PointerEventData enentData) {
        Exit();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (BeginDrag != null) {
            BeginDrag(material);
        }


    }
    public void OnDrag(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.isValid && eventData.pointerEnter.tag == "MaterialPanel") {
            if (Mathf.Abs(eventData.delta.y) > 10f && (Mathf.Abs(eventData.delta.y) >= (Mathf.Abs(eventData.delta.x) * 2f))) {
                parentScrollRect.velocity = new Vector2(0f, eventData.delta.y * 30f);
                Drag();
            }
        }


    }
    public void OnEndDrag(PointerEventData eventData) {
        if (EndDrag != null) {
            EndDrag(material, eventData);
        }
    }



}
