using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	
	public float damage;
	public LayerMask botLayer;
	public LayerMask wallLayer;
	public GameObject spark_effect;
	public static Vector3 hitPoint;

	public AudioSource shootingSound;
	public AudioSource subShootingSound;

	protected float timer;

	protected void NotifyNearby(Vector3 hitpoint){
		Collider[] colliders = Physics.OverlapSphere(hitpoint, 10f, botLayer);
		foreach(Collider c in colliders){
			Enemy_AI ai = c.gameObject.GetComponent<Enemy_AI>();

			Vector3 direction = c.transform.position - hitpoint;
			Ray ray = new Ray(hitpoint, direction.normalized);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 10f) && hit.collider.gameObject == c.gameObject){
				if(ai != null)
					ai._state = State.Notified;
			}
		}
	}
}
