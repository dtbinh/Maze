using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBot_AI : Aggressive_AI {
	public float attackRate;
	bool escaping;
	BlockNode escapeNode;

	// Update is called once per frame
	void Update () {
		_state = (PlayerHealth.isAlive)?_state:State.Idle;
		pathfinding.enabled = _state == State.Notified;

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Patrol:
			Patrol();
			break;
		case State.Notified:
			Notified();
			if (PlayerStatus.status==1) _state = State.Warning;
			break;
		case State.Warning:
			Warning();
			break;
		case State.Idle:
			break;
		}
	}

	void Init(){
		_state = State.Patrol;
	}

	void Notified(){
		if(playerInSight)
			lookAt(player.transform.position);
		scanForPlayer();
		skin.material.color = Color.yellow;
		distanceBetweenPlayer = Vector3.Distance(player.transform.position, transform.position);
		timer -= Time.deltaTime;
		if(distanceBetweenPlayer<attackRange && timer <=0){
			timer = attackRate;
			Attack();
		}
	}

	/* Escape from the player
	 */
	void Warning(){
		skin.material.color = Color.green;
		if (!escaping) {
			var neighbours = blockGridMap.GetNeighbours (transform.position);
			escapeNode = blockGridMap.GetBlock (transform.position);
			foreach (BlockNode n in neighbours) {
				escapeNode = Vector3.Distance (escapeNode.worldPosition, player.transform.position) > Vector3.Distance (n.worldPosition, player.transform.position) ? escapeNode : n;
			}
		}
		lookAt(escapeNode.worldPosition);
		transform.position = Vector3.MoveTowards(transform.position, escapeNode.worldPosition, speed * Time.deltaTime);

		escaping = !Equals (transform.position, escapeNode.worldPosition);

		if(PlayerStatus.status!=1){
			_state = State.Patrol;
			escaping = false;
			patrolling = false;
		}
	}

	/* Attack the player
	 */
	void Attack(){	
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange, targetLayer)){
			GameObject go = hit.collider.gameObject;
			if(go.tag == "Player"){
				go.GetComponent<PlayerHealth>().TakeDamage(damage + GameMaster.botPowerUpgrade);
			}
		}
	}


}
