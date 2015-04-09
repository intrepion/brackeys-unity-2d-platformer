using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour
{
	public int moveSpeed = 230;

	// Update is called once per frame
	void Update ()
	{
		transform.Translate (Vector3.right * Time.deltaTime * this.moveSpeed);
		Destroy (gameObject, 1);
	}
}
