using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LasserGun : Gun {
	public float damageRatius;
	public float damageRate;
	public Material[] lasserMaterial;
	public Color[] chargerColour;
	public AudioSource chargingSound;
	public AudioSource chargingSoundMax;
	public Slider chargerBar;
	public Image chargerLevelColour;
	public float maxCharge;
	public CanvasGroup chargerCanvas;
	float chargeTimer = 0;
	float extraDamageRatius;
	float extraDamage;
	float noiseLevel = 0.1f;
	bool chargedFiring;

	const int MAX_LINE_RANGE = 30;

	LineRenderer line;

	void Start(){
		chargingSound.SetScheduledStartTime (0.1f);
		line = GetComponent<LineRenderer>();
		line.enabled = false;
		chargerBar.maxValue = maxCharge-1;
	}

	void OnEnable(){
		if(line!=null){
			line.enabled = false;
		}
	}

	void Update(){
		timer -= Time.deltaTime;
		chargerBar.value = chargeTimer;
		if(Input.GetMouseButton(0) && !chargedFiring){
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
		if(Input.GetMouseButton(1) && !chargedFiring){
			StopCoroutine("FireChargedLaser");
			StartCoroutine("FireChargedLaser");
		}
	}

	public bool isChargedFiring{
		get{return chargedFiring;}
	}

	IEnumerator FireLaser(){
		if(!chargedFiring){
			line.enabled = true;
			while(Input.GetMouseButton(0)){
				shootingSound.Play ();
				Ray ray = new Ray(transform.position, transform.forward);
				RaycastHit hit;
				line.SetPosition(0, ray.origin);
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayer)){
					LineNoise(ray.origin, hit.point);
					if(spark_effect != null)
						Destroy(Instantiate(spark_effect, hit.point, Camera.main.transform.rotation),0.3f);
					if(timer <= 0){
						RaycastHit[] hits = Physics.SphereCastAll(ray, damageRatius, Vector3.Distance(transform.position,hit.point), botLayer);
						foreach(RaycastHit h in hits){
							h.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
							Destroy(Instantiate(spark_effect, h.point, Camera.main.transform.rotation),0.3f);
							NotifyNearby(h.point);
						}
						timer = damageRate;
					}
				}
				yield return null;
			}

			shootingSound.Stop ();
			line.enabled = false;
		}
	}

	IEnumerator FireChargedLaser(){
		if(!chargedFiring){
			chargerCanvas.alpha = 1;
			chargedFiring = true;
			while(Input.GetMouseButton(1)){
				if (chargeTimer <= maxCharge-1) {
					// Charing sound
					if (!chargingSound.isPlaying)
						chargingSound.Play ();
					// Ensure charger does not go overcharge
					chargeTimer = chargeTimer > maxCharge ? maxCharge : chargeTimer + Time.deltaTime;
					extraDamage = chargeTimer * 5f; 
					extraDamageRatius = chargeTimer / 2f;
					// Laser colour change
					line.material = lasserMaterial [Mathf.RoundToInt (chargeTimer)];
					// Charger bar colour change
					chargerLevelColour.color = chargerColour[Mathf.RoundToInt (chargeTimer)];
					// Noise level of the laser
					noiseLevel = chargeTimer / 5f;
				} else {
					if(!chargingSoundMax.isPlaying)
						chargingSoundMax.Play ();
				}

				yield return null;
			}
			chargingSound.Stop ();
			chargingSoundMax.Stop ();
			line.enabled = true;
			line.SetWidth(extraDamageRatius/10,extraDamageRatius/10);

			while(chargeTimer >= 0){
				if(!subShootingSound.isPlaying)
					subShootingSound.Play ();
				chargeTimer -= Time.deltaTime;
				Ray ray = new Ray(transform.position, transform.forward);
				RaycastHit hit;
				line.SetPosition(0, ray.origin);

				if(Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayer)){
					LineNoise(ray.origin, hit.point);
					Destroy(Instantiate(spark_effect, hit.point, Camera.main.transform.rotation),0.3f);
					if(timer <= 0){
						RaycastHit[] hits = Physics.SphereCastAll(ray, damageRatius + extraDamageRatius, Vector3.Distance(transform.position,hit.point), botLayer);
						foreach(RaycastHit h in hits){
							h.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage + extraDamage);
							Destroy(Instantiate(spark_effect, h.point, Camera.main.transform.rotation),0.3f);
							NotifyNearby(h.point);
						}
						timer = damageRate;
					}
				}				
				yield return null;
			}
			chargerCanvas.alpha = 0;
			chargerLevelColour.color = chargerColour[0];
			noiseLevel = 0.1f;
			line.material = lasserMaterial [0];
			line.SetWidth (0.02f, 0.02f);
			line.enabled = false;
			chargedFiring = false;
		}
	}

	void LineNoise(Vector3 start, Vector3 end){
		int range = (int)Vector3.Distance (start,end);
		if (range >= MAX_LINE_RANGE) {
			range = MAX_LINE_RANGE - 2;
		}

		for (int i = 1; i < range; i++) {
			Vector3 pos = Vector3.Lerp (start, end, i / 10.0f);
			pos.x += Random.Range (-noiseLevel, noiseLevel);
			pos.y += Random.Range (-noiseLevel, noiseLevel);
			line.SetPosition (i, pos);
		}

		for (int i = range; i < MAX_LINE_RANGE; i++) {
			line.SetPosition (i, end);
		}
	}
}
