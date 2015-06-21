using UnityEngine;
using System.Collections;

public class BigBullet : MonoBehaviour {

	public LayerMask colliderLayer;
	public GameObject explosion_effect;
	public int damage;
	public float aoeRange;
	public float travelSpeed;

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(transform.forward * travelSpeed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Wall" || c.tag == "SpeedBot" || c.tag == "AttackBot" || c.tag == "DefenceBot") {
			if (c.GetComponent<EnemyHealth> ()) {
				c.GetComponent<EnemyHealth> ().TakeDamage (damage);
			}
			Explode ();
		}
	}

	void Explode(){
		Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRange, colliderLayer);

		foreach(Collider c in colliders){
			c.gameObject.GetComponent<Enemy_AI>().notified = true;
			c.GetComponent<EnemyHealth>().TakeDamage(damage);
		}

		Instantiate(explosion_effect, transform.position, Camera.main.transform.rotation);
		Destroy(gameObject);
	}
}
