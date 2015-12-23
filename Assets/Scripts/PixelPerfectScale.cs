using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectScale : MonoBehaviour {

	private int screenVerticalPixels = 200;
	public bool preferUncropped = true;
	
	private float screenPixelsY = 0;
	private bool currentCropped = false;
	
	void Update () {
		if (screenPixelsY != (float)Screen.height || currentCropped != preferUncropped) {
			screenPixelsY = (float)Screen.height;
			currentCropped = preferUncropped;
			
			float screenRatio = screenPixelsY / screenVerticalPixels;
			float ratio = 0;
			
			if (preferUncropped) {
				ratio = Mathf.Floor(screenRatio) / screenRatio;
			} else {
				ratio = Mathf.Ceil(screenRatio) / screenRatio;
			}
			//this used to work by multiplying by ratio, but the best ratio in preview play mode is 0.7794.
			transform.localScale = Vector3.one * 0.7794f;
		}
	}
}