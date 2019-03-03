//using System;
//using System.Collections;
//using System.Collections.Generic;

//using UnityEngine;
//using UnityEngine.EventSystems;


//public class Model : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {



//    private Rigidbody rb;
//    private BoxCollider collider;

//    private float counter;
//    private bool counting;

//    private bool isDrag;


//    private void OnEnable() {
//        rb = GetComponent<Rigidbody>();
//        collider = GetComponent<BoxCollider>();
//    }

//    private void Update() {
//        if (counting && !isDrag) {
//            counter += Time.deltaTime;
//        }
//        if (counter > 2f) {
//            Destroy(gameObject);
//        }

//    }
//    public void OnPointerEnter(PointerEventData eventData) {
//        counting = true;
//    }

//    public void OnPointerExit(PointerEventData enentData) {
//        counting = false;
//        counter = 0f;
//    }

//    public void OnBeginDrag(PointerEventData eventData) {
//        isDrag = true;
//        rb.isKinematic = true;

//    }

//    public void OnDrag(PointerEventData eventData) {
//        if(Input.touchCount==1){
//            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
//            float enter = 0.0f;
//            if (GroundPlaneManager.instance.hitTestGroundPlane.Raycast(ray, out enter)) {
//                Vector3 hitPoint = ray.GetPoint(enter);
//                transform.position = hitPoint;
//            }
//        }
//        if(Input.touchCount==2){
//            transform.rotation *= Quaternion.AngleAxis(eventData.delta.x, Vector3.up);
//        }
//    }

//    public void OnEndDrag(PointerEventData eventData) {
//        rb.isKinematic = false;
//        rb.WakeUp();
//    }

//}
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine;
using TouchScript.Behaviors;
using TouchScript.Gestures.TransformGestures;

[RequireComponent(typeof(Transformer))]
[RequireComponent(typeof(TransformGesture))]
[RequireComponent(typeof(PinnedTransformGesture))]
//[RequireComponent(typeof(BoxCollider))]
//[RequireComponent(typeof(Rigidbody))]
public class Model : MonoBehaviour {

    public GameObject model;
    private PinnedTransformGesture rotateGesture;
    private TransformGesture transformGesture;
    private Transformer transformer;
    private Rigidbody rb;



    public void Setup() {
        rotateGesture = GetComponent<PinnedTransformGesture>();
        rotateGesture.Transformed += transformedHandler;

        // The gesture
        transformGesture = GetComponent<TransformGesture>();
        // Transformer component actually MOVES the object
        transformer = GetComponent<Transformer>();
        transformer.enabled = false;

        // Subscribe to gesture events
        transformGesture.TransformStarted += transformStartedHandler;
        transformGesture.TransformCompleted += transformCompletedHandler;

        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    private void OnDisable() {
        rotateGesture.Transformed -= transformedHandler;
        transformGesture.TransformStarted -= transformStartedHandler;
        transformGesture.TransformCompleted -= transformCompletedHandler;
        GetComponent<TapGesture>().Tapped -= tappedHandler;
    }

    private void transformedHandler(object sender, System.EventArgs e) {
        transform.rotation *= Quaternion.AngleAxis(rotateGesture.DeltaRotation, rotateGesture.RotationAxis);
    }



    private void transformStartedHandler(object sender, EventArgs e) {
        //When movement starts we need to tell physics that now WE are moving this object manually
        rb = GetComponentInChildren<Rigidbody>();
        rb.isKinematic = true;
        transformer.enabled = true;
    }

    private void transformCompletedHandler(object sender, EventArgs e) {
        transformer.enabled = false;
        rb.isKinematic = false;
        rb.WakeUp();
    }

    private void tappedHandler(object sender, EventArgs e) {
        var gesture = sender as TapGesture;
        HitData hit = gesture.GetScreenPositionHitData();

        Destroy(hit.RaycastHit.collider.gameObject.transform.parent.gameObject);
    }

}