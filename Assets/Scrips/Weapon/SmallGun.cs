using UnityEngine;
using System.Collections;

public class SmallGun : Gun {
	public float fireRate;
	public float subFireRate;
	public GameObject small_bullet_prefab;
	public GameObject big_bullet_prefab;
	public WeaponManager weaponManager;

	void Start(){
		weaponManager.cooldownBar.maxValue = subFireRate;
	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if(Input.GetMouseButton(0) && timer <= 0){
			timer = fireRate;
			shootingSound.Play ();
			Ray ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward*0.1f, Camera.main.transform.forward);
			RaycastHit hitInfo;
			
			if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)){
				hitPoint = hitInfo.point;
				GameObject go = hitInfo.collider.gameObject;
				
				EnemyHealth h = go.GetComponent<EnemyHealth>();
				Enemy_AI ai = go.GetComponent<Enemy_AI>();
				if(h != null){
					h.TakeDamage(damage + PlayerStatus.damageIncreased);
					if(ai != null)
						ai._state = State.Notified;
					NotifyNearby(hitPoint);
				}
				
				if(spark_effect != null)
					Destroy(Instantiate(spark_effect, hitPoint, Camera.main.transform.rotation),0.3f);
			}
			
			Destroy(Instantiate(small_bullet_prefab, transform.position + transform.forward, transform.rotation),.5f);
		}

		if(Input.GetMouseButton(1) && weaponManager.GetBigBulletTimer >= subFireRate){
			weaponManager.GetBigBulletTimer = 0;
			subShootingSound.Play ();
			Destroy(Instantiate(big_bullet_prefab, transform.position, Camera.main.transform.rotation), 5);
		}
	}
}
