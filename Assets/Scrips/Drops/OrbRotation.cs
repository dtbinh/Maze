using UnityEngine;

public class OrbRotation : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(transform.parent.position, new Vector3(0, 1, 0), 200f * Time.deltaTime);
	}
}
