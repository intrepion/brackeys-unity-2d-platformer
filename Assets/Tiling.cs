using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
	public int offsetX = 2;				// the offset so that we don't get any weird errors

	// this are used for checking if we need to instantiate stuff
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;
	public bool reverseScale = false;	// used if the object is not tilable

	private float spriteWidth = 0f;		// the width of our element
	private Camera cam;
	private Transform myTransform;

	void Awake ()
	{
		this.cam = Camera.main;
		this.myTransform = transform;
	}

	// Use this for initialization
	void Start ()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		this.spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// does it still need buddies?  If not, do nothing
		if (this.hasALeftBuddy == false || this.hasARightBuddy == false) {
			// calculate the cameras extend (half the width) of what the camera can see in world coordinates
			float camHorizontalExtend = this.cam.orthographicSize * Screen.width / Screen.height;

			// calculate the x position where the camera can see the edge of the sprite (element)
			float edgeVisiblePositionRight = (this.myTransform.position.x + this.spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (this.myTransform.position.x - this.spriteWidth / 2) + camHorizontalExtend;

			// checking if we can see the edge of the element and then calling MakeNewBuddy if we can
			if (this.cam.transform.position.x >= edgeVisiblePositionRight - this.offsetX && this.hasARightBuddy == false) {
				MakeNewBuddy (1);
				this.hasARightBuddy = true;
			} else if (this.cam.transform.position.x <= edgeVisiblePositionLeft + this.offsetX && this.hasALeftBuddy == false) {
				MakeNewBuddy (-1);
				this.hasALeftBuddy = true;
			}
		}
	}

	// a function that creates a buddy on the side required
	void MakeNewBuddy (int rightOrLeft)
	{
		// calculating the new position for our new buddy
		Vector3 newPosition = new Vector3 (this.myTransform.position.x + this.spriteWidth * rightOrLeft, this.myTransform.position.y, this.myTransform.position.z);
		// instantiating our new buddy and storing him in a variable
		Transform newBuddy = Instantiate (this.myTransform, newPosition, this.myTransform.rotation) as Transform;

		// if not tilable, let's reverse the size of our object to get rid of ugly seams
		if (this.reverseScale == true) {
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = this.myTransform.parent;
		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true;
		}
	}
}
