using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {

	public Transform[] backgrounds;
	private float smoothing = 30;
	
	private BoxCollider2D levelCollider;
	private float levelSize;
	
	private Controller2D Controller2D;
	
	//This is static and I dont like it....
	private float cameraWidth = 16f;
	
	private float cameraPosition;
	
	void Awake () {
	}

	void Start () {
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x;
		levelCollider = this.GetComponent<BoxCollider2D>();
		levelSize = levelCollider.bounds.size.x - (cameraWidth / 2);
		Controller2D = GameObject.FindObjectOfType<Controller2D>();
	}
	
	
	void Update () {
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x;
		int directionX = Controller2D.collisions.faceDir;
		
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
