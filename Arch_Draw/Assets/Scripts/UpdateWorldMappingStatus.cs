using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class UpdateWorldMappingStatus : MonoBehaviour 
{

	public Text text;
	public Text tracking;
    public Text reason;


	// Use this for initialization
	void Start () 
	{
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += CheckWorldMapStatus;
	}

	void CheckWorldMapStatus(UnityARCamera cam)
	{
        char[] trim = { '.' };
        text.text = cam.worldMappingStatus.ToString ();
        tracking.text = cam.trackingState.ToString(); 
        reason.text = cam.trackingReason.ToString();

        if(cam.trackingReason == ARTrackingStateReason.ARTrackingStateReasonNone){

        }
    }

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= CheckWorldMapStatus;
	}

}
