using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float fireRate = 0f;
	public float Damage = 10f;
	public LayerMask whatToHit;
	public Transform BulletTrailPrefab;
	public float effectSpawnRate = 10f;
	public Transform MuzzleFlashPrefab;

	private float timeToFire = 0;
	private Transform firePoint;
	private float timeToSpawnEffect = 0f;

	// Use this for initialization
	void Awake ()
	{
		this.firePoint = transform.FindChild ("Fire Point");
		if (this.firePoint == null) {
			Debug.LogError ("No Fire Point?  WHAT?!");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				this.Shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > this.timeToFire) {
				this.timeToFire = Time.time + 1 / this.fireRate;
				this.Shoot ();
			}
		}
	}

	void Shoot ()
	{
		Vector2 screenToWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 mousePosition = new Vector2 (screenToWorldPoint.x, screenToWorldPoint.y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		if (Time.time >= this.timeToSpawnEffect) {
			this.Effect ();
			this.timeToSpawnEffect = Time.time + 1 / this.effectSpawnRate;
		}
		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			Debug.Log ("We hit " + hit.collider.name + " and did " + this.Damage + " damage.");
		}
	}

	void Effect ()
	{
		Instantiate (this.BulletTrailPrefab, this.firePoint.position, firePoint.rotation);
		Transform clone = (Transform) Instantiate (this.MuzzleFlashPrefab, this.firePoint.position, firePoint.rotation);
		clone.parent = this.firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
	}
}
