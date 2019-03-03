using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ModelPost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public MyModel model { get; private set; }
    private ScrollRect parentScrollRect;

    public void Setup(MyModel model, ScrollRect parent) {
        this.model = model;
        GetComponent<Image>().sprite = model.img;
        parentScrollRect = parent;
    }



    public static Action<MyModel> Enter;
    public static Action Exit;
    public static Action<MyModel> BeginDrag;
    public static Action Drag;
    public static Action<MyModel, PointerEventData> EndDrag;

    public void OnPointerEnter(PointerEventData eventData) {
        Enter(model);
    }

    public void OnPointerExit(PointerEventData enentData) {
        Exit();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (BeginDrag != null) {
            BeginDrag(model);
        }


    }
    public void OnDrag(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.isValid && eventData.pointerEnter.tag == "ModlePanel") {
            if (Mathf.Abs(eventData.delta.y) > 10f && (Mathf.Abs(eventData.delta.y) >= (Mathf.Abs(eventData.delta.x) * 2f))) {
                parentScrollRect.velocity = new Vector2(0f, eventData.delta.y * 30f);
                Drag();
            }
        }

    }
    public void OnEndDrag(PointerEventData eventData) {
        if (EndDrag != null) {
            EndDrag(model, eventData);
        }
    }



}