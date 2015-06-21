using UnityEngine;
using System.Collections;

public class PickupEffect : MonoBehaviour {
	public int healAmount = 10;
	public bool isPotion;
	public AudioSource sound;

	PlayerStatus playerStatus;
	PickupGenerator pickupManager;
	PlayerHealth playerHealth;
	SphereCollider sc;

	// Use this for initialization
	void Awake () {
		playerStatus = GameObject.Find("_Player").GetComponent<PlayerStatus>();
		pickupManager = GameObject.Find("_PickupGenerator").GetComponent<PickupGenerator>();
		playerHealth = GameObject.Find("_Player").GetComponent<PlayerHealth>();
		sc = GetComponent<SphereCollider> ();
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Player"){
			if(isPotion){
				if (PlayerHealth.currentHealth < playerHealth.baseHealth) {
					PlayerHealth.currentHealth += healAmount;
				}
			} else {
				playerStatus.Buff (gameObject.name);
			}
			sound.Play ();
			sc.enabled = false;
			foreach(Transform t in transform){
				t.gameObject.SetActive (false);
			}
			Destroy(gameObject, 2f);
		}

	}

	void OnDestroy(){
		pickupManager.PickupTaken(gameObject.name, gameObject.transform.position);
	}
}
