using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

	public static ParallaxScrolling parallaxScrolling;

	public Transform[] backgrounds;
	//private float smoothing = 20;
	
	private Collider2D levelCollider;
	private float levelSizeX;
	private float levelSizeY;
	[HideInInspector] public Bounds levelBounds;
	
	//this works in 2:1 aspect ratio.
	private float cameraWidth;
	
	private Vector2 cameraPosition;

	void Start () {
		parallaxScrolling = GetComponent<ParallaxScrolling>();
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		cameraWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2 * 2;
		levelCollider = this.GetComponent<Collider2D>();
		levelSizeX = levelCollider.bounds.size.x;
        levelSizeY = levelCollider.bounds.size.y;
		levelBounds = levelCollider.bounds;
	}
	
	//called from the camera follow script.
	public void Scrolling () {
		cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		for (var i = 0; i < backgrounds.Length; i++) {
			float backgroundSizeX = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x * 1;
			float backgroundSizeY = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.y * 1;
			
			float maxBackgroundPositionY = levelSizeY - backgroundSizeY;
            float distanceToMoveX = levelSizeX - cameraWidth;
            float distanceToMoveY = levelSizeX - (cameraWidth / 2);

            float rateOfMovementX = (backgroundSizeX - cameraWidth) / distanceToMoveX;
			float rateOfMovementY = (backgroundSizeY - (cameraWidth / 2)) / distanceToMoveY;

            var backgroundTargetPositionX = ((cameraPosition.x - cameraWidth / 2) + (rateOfMovementX * (cameraWidth / 2))) - (cameraPosition.x * (rateOfMovementX));
            var backgroundTargetPositionY = ((cameraPosition.y - cameraWidth / 4) + (rateOfMovementY * (cameraWidth / 4))) - (cameraPosition.y * (rateOfMovementY));
			
			backgrounds[i].position = Vector3.Lerp(
                //From:
				backgrounds[i].position,
                //To:
				new Vector3(backgroundTargetPositionX, Mathf.Clamp (backgroundTargetPositionY, 0, maxBackgroundPositionY), backgrounds[i].position.z),
                //Smoothing:
				//smoothing * Time.smoothDeltaTime);
                1);
    }
		cameraPosition = transform.position;
	}
}
