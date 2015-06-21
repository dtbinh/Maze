using UnityEngine;

public class DefenceBot_AI : Aggressive_AI{

	public float OrbRotationSpeed;
	public float chargeTime;
	public float chargeSpeed;
	public float warningTime;
	public float impactForce;
	public AudioSource chargeSound;

	Transform[] orbs;
	bool targetMarked;
	bool attacked;
	Vector3 targetPosition;
	Rigidbody rigidBody;

	// Update is called once per frame
	void Update () {
		_state = (PlayerHealth.isAlive)?_state:State.Idle;
		timer -= Time.deltaTime;
		pathfinding.enabled = (_state == State.Notified) && !targetMarked;

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Patrol:
			Patrol();
			break;
		case State.Notified:
			Notified();
			break;
		case State.Warning:
			Warning();
			break;
		case State.Idle:
			break;
		}
		RotateOrbs();
	}

	void Init(){
		orbs = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();
		rigidBody = GetComponent<Rigidbody>();
		_state = State.Patrol;
	}

	void OnCollisionEnter(Collision c){
		if(c.collider.tag == "Player" && targetMarked){
			ImpactReceiver forceReceiver = c.collider.GetComponent<ImpactReceiver>();
			if (forceReceiver && !attacked){
				attacked = true;
				c.collider.GetComponent<PlayerHealth>().TakeDamage (damage + GameMaster.botPowerUpgrade);
				forceReceiver.AddImpact (transform.forward, impactForce);

				_state = State.Warning;
				timer = warningTime;
			}
		}
	}

	void Notified(){
		if(!targetMarked){
			skin.material.color = Color.yellow;
			lookAt(player.transform.position);
			scanForPlayer();
			TargetMark();
		}else if(timer < 0){
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
			if (!chargeSound.isPlaying)
				chargeSound.Play ();
			if(transform.position == targetPosition){
				_state = State.Warning;
				timer = warningTime;
			}
		}
	}
	void Warning(){
		targetMarked = false;
		rigidBody.freezeRotation = false;
		skin.material.color = Color.green;
		if(timer < 0){
			_state = State.Notified;
			attacked = false;
		}
	}

	void RotateOrbs(){
		if(_state != State.Init)
			foreach(Transform t in orbs){
				t.RotateAround(transform.position, new Vector3(0, 1, 0), OrbRotationSpeed * Time.deltaTime);
			}
	}

	void TargetMark(){
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange, targetLayer)){
			if(hit.transform.tag == "Player"){
				skin.material.color = Color.red;
				rigidBody.freezeRotation = true;
				targetMarked = true;
				targetPosition = hit.point;
				timer = chargeTime;
				lookAt(hit.point);
			}
		}
	}
}
