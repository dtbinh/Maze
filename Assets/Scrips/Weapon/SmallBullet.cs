using UnityEngine;
using System.Collections;

public class SmallBullet : MonoBehaviour {

	public float travelSpeed;

	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, SmallGun.hitPoint, Time.deltaTime * travelSpeed);
		if(transform.position == SmallGun.hitPoint)
			Destroy(gameObject);
	}
}
