using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

	public static ParallaxScrolling parallaxScrolling;

	public Transform[] backgrounds;
	public float smoothing = 20;
	
	private Collider2D levelCollider;
	private float levelSizeX;
	private float levelSizeY;
	[HideInInspector]
	public Bounds levelBounds;
	//private Controller2D Controller2D;
	
	//this works in 2:1 aspect ratio.
	private float cameraWidth;
	
	private Vector2 cameraPosition;
	
	void Awake () {
	}

	void Start () {
		parallaxScrolling = GetComponent<ParallaxScrolling>();
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		cameraWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2 * 2;
		levelCollider = this.GetComponent<Collider2D>();
		levelSizeX = levelCollider.bounds.size.x - (cameraWidth / 2);
		levelSizeY = levelCollider.bounds.size.y;
		levelBounds = levelCollider.bounds;
		//Controller2D = GameObject.FindObjectOfType<Controller2D>();
	}
	
	//called from the camera follow script.
	public void Scrolling () {
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		for (var i = 0; i < backgrounds.Length; i++) {
			float backgroundSizeX = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x * 1;
			float backgroundSizeY = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.y * 1;
			
			float maxBackgroundPositionY = levelSizeY - backgroundSizeY;
//			float maxCameraPositionY = levelSizeY - (cameraWidth / 4);

			float rateOfMovementX = (backgroundSizeX - cameraWidth) / levelSizeX;
			float rateOfMovementY = (backgroundSizeY - (cameraWidth / 2)) / levelSizeY;

			var backgroundTargetPositionX = ((cameraPosition.x - cameraWidth / 2)) - (cameraPosition.x * (rateOfMovementX));
			var backgroundTargetPositionY = ((cameraPosition.y - cameraWidth / 4) + (rateOfMovementY * (cameraWidth / 4))) - (cameraPosition.y * (rateOfMovementY));
			
			backgrounds[i].position = Vector3.Lerp(
				backgrounds[i].position, //from
				new Vector3(backgroundTargetPositionX, Mathf.Clamp (backgroundTargetPositionY, 0, maxBackgroundPositionY), backgrounds[i].position.z), //to
				smoothing * Time.deltaTime);
		}
		cameraPosition = transform.position;
	}
}
