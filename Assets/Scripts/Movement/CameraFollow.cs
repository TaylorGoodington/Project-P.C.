using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//Singleton Pattern
	public static CameraFollow cameraFollow;
	
	[HideInInspector]
	public Controller2D target;
	
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;
	
	private float cameraWidth = 4;
	private float cameraHeight = 2;
	
	private ParallaxScrolling parallaxScrolling;
	
	FocusArea focusArea;
	
	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;
	
	bool lookAheadStopped;
	
	void Start() {
		focusArea = new FocusArea (target.boxCollider.bounds, focusAreaSize);
		//this will be called by something else at some point....
		UpdateTarget();
		parallaxScrolling = GameObject.FindObjectOfType<ParallaxScrolling>().GetComponent<ParallaxScrolling>();
	}
	
	public void UpdateTarget () {
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
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
		
//		currentLookAheadX = Mathf.Clamp(Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX),
//		                    (ParallaxScrolling.parallaxScrolling.levelBounds.min.x + 8f), ParallaxScrolling.parallaxScrolling.levelBounds.max.x - 8f);
		
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
		
		//clamps to current level bounds in y.
		focusPosition.y = Mathf.Clamp(Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime), 
		                              (parallaxScrolling.levelBounds.min.y + cameraHeight), parallaxScrolling.levelBounds.max.y - cameraHeight);


		//old code that doesnt clamp.
//		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
//		transform.position = (Vector3)focusPosition + Vector3.forward * -10;		
		
		focusPosition += Vector2.right * currentLookAheadX;
		transform.position = new Vector3 (Mathf.Clamp (focusPosition.x, parallaxScrolling.levelBounds.min.x + 4, 
																		parallaxScrolling.levelBounds.max.x - 4), focusPosition.y, -10);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);
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