using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	//Singleton Pattern
	public static CameraFollow cameraFollow;
	
	public LayerMask levelBounds;
	
//	[HideInInspector]
	public Controller2D target;
	
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;
	
	private float cameraHeight;
	
	private ParallaxScrolling parallaxScrolling;
	
	FocusArea focusArea;
	
	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;
	
	bool lookAheadStopped;
		
	void Start() {
		cameraFollow = GetComponent<CameraFollow>();
		focusArea = new FocusArea (target.boxCollider.bounds, focusAreaSize);
		//these will be called by something else at some point....
		UpdateTarget();
		parallaxScrolling = GameObject.FindObjectOfType<ParallaxScrolling>().GetComponent<ParallaxScrolling>();
		cameraHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize;
		
		FindBounds();
	}
	
	public void UpdateTarget () {
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
	}
	
	public void FindBounds () {
		Vector2 topRight = new Vector2 (transform.position.x + (cameraHeight * 2), transform.position.y + cameraHeight);
		Vector2 topLeft = new Vector2 (transform.position.x - (cameraHeight * 2), transform.position.y + cameraHeight);
		Vector2 bottomRight = new Vector2 (transform.position.x + (cameraHeight * 2), transform.position.y - cameraHeight);
		Vector2 bottomLeft = new Vector2 (transform.position.x - (cameraHeight * 2), transform.position.y - cameraHeight);
		Vector2 rightTop = new Vector2 (transform.position.x + (cameraHeight * 2), transform.position.y + cameraHeight);
		Vector2 leftTop = new Vector2 (transform.position.x - (cameraHeight * 2), transform.position.y + cameraHeight);
		Vector2 rightBottom = new Vector2 (transform.position.x + (cameraHeight * 2), transform.position.y - cameraHeight);
		Vector2 leftBottom = new Vector2 (transform.position.x - (cameraHeight * 2), transform.position.y - cameraHeight);
		
//		List<Vector2> cameraCorners = new List<Vector2>();
//		cameraCorners.Add (topRight);
//		cameraCorners.Add (topLeft);
//		
//		Debug.Log (cameraCorners[1].x);
	//I dont think making a list is working, guess i need to do it one at a time.
		
		RaycastHit2D topRightHit = Physics2D.Raycast(topRight, Vector2.right, 5000, levelBounds);
		if (topRightHit) {
			Vector2 topRightHitPoint = topRightHit.point;
		}
	}
	
	void LateUpdate() {
		focusArea.Update (target.boxCollider.bounds);
		
		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
		
		if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * lookAheadDstX;
			}
			else {
				if (!lookAheadStopped) {
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX)/4f;
				}
			}
		}
		
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
		
		//This section allows the camera to pan up or down depending on the direction pressed.
		if (GameControl.gameControl.AnyOpenMenus() == false) {
			float panDirection = Mathf.Sign(Input.GetAxisRaw("Vertical"));
			if (Input.GetAxisRaw ("Vertical") > 0.4f || Input.GetAxisRaw ("Vertical") < -0.4f) {
				focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusArea.center.y + ((focusAreaSize.y / 2) * panDirection), ref smoothVelocityY, verticalSmoothTime);
			} else if (Input.GetAxisRaw ("Vertical") < 0.4f || Input.GetAxisRaw ("Vertical") > -0.4f) {
				focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
			}	
		}
		
		
		focusPosition += Vector2.right * currentLookAheadX;
		
//		transform.position = new Vector3 (Mathf.Clamp (focusPosition.x, parallaxScrolling.levelBounds.min.x + (cameraHeight * 2), 
//		                                                               parallaxScrolling.levelBounds.max.x - (cameraHeight * 2)), 
//		                                  Mathf.Clamp(focusPosition.y,(parallaxScrolling.levelBounds.min.y + cameraHeight), 
//		                                  							   parallaxScrolling.levelBounds.max.y - cameraHeight), -10);
		                                  							   
		transform.position = new Vector3 (Mathf.Clamp (focusPosition.x, parallaxScrolling.levelBounds.min.x + (cameraHeight * 2), 
		                                               parallaxScrolling.levelBounds.max.x - (cameraHeight * 2)), 
		                                  Mathf.Clamp(focusPosition.y,(parallaxScrolling.levelBounds.min.y + cameraHeight), 
		            parallaxScrolling.levelBounds.max.y - cameraHeight), -10);
		            
		parallaxScrolling.Scrolling();
	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);
	}
	
	public void PanCamera () {
		float panDirection = Mathf.Sign(Input.GetAxisRaw("Vertical"));
		if (panDirection == -1) {
		
		}
		Debug.Log(panDirection);
	}
	
	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left,right;
		float top,bottom;
		
		
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;
			
			velocity = Vector2.zero;
			center = new Vector2((left+right)/2,(top +bottom)/2);
		}
		
		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;
			
			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			center = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}
	
}