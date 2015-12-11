using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {

	public static ParallaxScrolling parallaxScrolling;

	public Transform[] backgrounds;
	private float smoothing = 30;
	
	private BoxCollider2D levelCollider;
	private float levelSize;
	[HideInInspector]
	public Bounds levelBounds;
	private Controller2D Controller2D;
	
	//this works in 2:1 aspect ratio.
	private float cameraWidth;
	
	private float cameraPosition;
	private GameObject cameraObject;
	
	void Awake () {
	}

	void Start () {
		parallaxScrolling = GetComponent<ParallaxScrolling>();
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x;
		cameraWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2 * 2;
		levelCollider = this.GetComponent<BoxCollider2D>();
		levelSize = levelCollider.bounds.size.x - (cameraWidth / 2);
		levelBounds = levelCollider.bounds;
		Controller2D = GameObject.FindObjectOfType<Controller2D>();	
	}
	
	
	void Update () {
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x;
		for (var i = 0; i < backgrounds.Length; i++) {
			float backgroundSize = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
			float rateOfMovement = (backgroundSize - cameraWidth) / levelSize;
			var backgroundTargetPosition = (cameraPosition - cameraWidth / 2) - (cameraPosition * (rateOfMovement));
			backgrounds[i].position = Vector3.Lerp(
				backgrounds[i].position, //from
				new Vector3(backgroundTargetPosition, backgrounds[i].position.y, backgrounds[i].position.z), //to
				smoothing * Time.deltaTime);
		}
		cameraPosition = transform.position.x;
	}
}
