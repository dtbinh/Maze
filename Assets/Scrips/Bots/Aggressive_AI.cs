using UnityEngine;
using System.Collections;

public class Aggressive_AI : Enemy_AI {
	public float attackRange;
	public float frontSightDistance;
	public float backSightDistance;
	public float obsticleAvoidingRadius;
	public float obsticleAvoidingDistance;
	public float fieldOfViewAngle = 200;
	public int damage;
	protected Pathfinding pathfinding;
	// Use this for initialization
	void Start () {
		pathfinding = GetComponent<Pathfinding>();
		pathfinding.enabled = false;
	}

	/*
	 * Patrol around the maze
	 */
	protected void Patrol(){
		skin.material.color = Color.white;
		if(!patrolling){
			patrolling = true;
			// Get neighbours from the current block
			var neighbours = blockGridMap.GetNeighbours(transform.position);
			// Check if more than 1 neighbours...
			if(neighbours.Count > 1){
				// Remove the last block where came from neighbours
				foreach(BlockNode n in neighbours){
					if(lastNode.worldPosition == n.worldPosition){
						neighbours.Remove(n);
						lastNode = blockGridMap.GetBlock(transform.position);
						break;
					}
				}
				// Update the new block to patrol
				patrolNode = neighbours[Random.Range(0, neighbours.Count)];
			}else{
				lastNode = blockGridMap.GetBlock(transform.position);
				patrolNode = neighbours[0];
			}
		}else{
			lookAt(patrolNode.worldPosition);
			AvoidObstacles ();
			scanForPlayer ();
			// Move to patrol block
			transform.position += transform.forward * speed * Time.deltaTime;
			// Stop patrolling when arrives at patrol block
			patrolling &= Vector3.Distance (transform.position, patrolNode.worldPosition) >= 3f;
		}
	}

	protected void AvoidObstacles(){
		RaycastHit hit;
		var ray = new Ray(transform.position, transform.forward);
		if(Physics.SphereCast(ray, obsticleAvoidingRadius, out hit, obsticleAvoidingDistance)){
			if(hit.transform.tag == transform.tag && (hit.point - transform.position) != transform.position - hit.point)
				transform.position += transform.right * speed * Time.deltaTime;
		}
	}

	protected void scanForPlayer(){
		// Create a vector from the enemy to the player and store the angle between it and forward.
		Vector3 direction = player.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
		// If the angle between forward and where the player is, is less than half the angle of view...
		if(angle < fieldOfViewAngle * 0.5f)
			CastRay(frontSightDistance, direction);
		else
			CastRay(backSightDistance, direction);
	}


	/*
	 * Cast ray to a direction
	 */
	void CastRay(float range, Vector3 direction){
		var ray = new Ray(transform.position, direction.normalized);
		RaycastHit hit;
		// ... and if a raycast towards the player hits something...
		if(Physics.Raycast(ray, out hit, range, targetLayer)){
			GameObject go = hit.collider.gameObject;
			// ... and if the raycast hits the player...
			if(go.tag == player.tag){
				// ... the player is in sight.
				playerInSight = true;
				_state = State.Notified;
			}else
				playerInSight = false;
		}
	}
}
