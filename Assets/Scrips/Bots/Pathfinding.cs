using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Pathfinding : MonoBehaviour {	
	public LayerMask obstacleLayer;
	Transform player;
	Vector3[] waypoints;
	Vector3[] curWaypoints;
	Grid grid;
	Aggressive_AI ai;
	int nextWaypoint;
	Vector3 currentWaypoint;
	bool searching;

	void Awake() {
		grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		ai = GetComponent<Aggressive_AI>();
	}

	void Update() {
		float distance = Vector3.Distance(transform.position, player.position);
		/*if(distance > 30f){
			ai._state = State.Patrol;
			ai.patrolling = false;
			StopSearching();
		}*/
		if(!ai.playerInSight){
			// Find path
			if(!searching){
				StartCoroutine("FindPath");
			}

			// if path is found
			if(waypoints != null && waypoints.Length > 0){
				currentWaypoint = waypoints[nextWaypoint];
				if(Vector3.Distance(transform.position, currentWaypoint) < .5f){
					nextWaypoint ++;
					if (nextWaypoint >= waypoints.Length-1) {
						nextWaypoint = waypoints.Length-1;
					}
					currentWaypoint = waypoints[nextWaypoint];
				}
				LookAt (currentWaypoint);
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, ai.speed * Time.deltaTime);
			}
		}else if(distance > ai.attackRange){
			StopSearching();
			LookAt (player.position);
			transform.Translate(Vector3.forward * ai.speed * Time.deltaTime);
		}
	}

	void StopSearching(){
		StopCoroutine("FindPath");
		waypoints = null;
		searching = false;
	}

	void LookAt(Vector3 pos){
		Quaternion angle;
		angle = pos != transform.position ? Quaternion.LookRotation (pos - transform.position) : Quaternion.identity;
		transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * ai.damping);
	}
	
	IEnumerator FindPath() {
		searching = true;
		Node startNode = grid.NodeFromWorldPoint(transform.position);
		Node playerNode = grid.NodeFromWorldPoint(player.position);

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);
		
		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);
			
			if (currentNode == playerNode) {
				RetracePath(startNode,playerNode);
				nextWaypoint = 0;
				yield return new WaitForSeconds(2);
				searching = false;
				yield break;
			}
			
			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)){
					continue;
				}
				
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, playerNode);
					neighbour.parent = currentNode;
					
					if (!openSet.Contains(neighbour)){
						openSet.Add(neighbour);
					}
				}
			}
		}
	}
	
	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] smoothedPath = SmoothPath(path);
		Array.Reverse(smoothedPath);
		//for debug
		grid.path = smoothedPath;
		waypoints = smoothedPath;
		//
	}

	Vector3[] SmoothPath(List<Node> originalPath){
		List<Vector3> smoothedPath = new List<Vector3>();
		smoothedPath.Add (originalPath[0].worldPosition);
		int inputIndex = 2;
		while(inputIndex < originalPath.Count){
			if(Physics.CheckCapsule(smoothedPath [smoothedPath.Count - 1], originalPath [inputIndex].worldPosition, .5f, obstacleLayer)){
				smoothedPath.Add (originalPath[inputIndex-1].worldPosition);
			}
			inputIndex++;
		}
		smoothedPath.Add (originalPath[originalPath.Count-1].worldPosition);
		return smoothedPath.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10*(dstX-dstY);
		return 14*dstX + 10*(dstY-dstX);
	}

	public void OnDrawGizmos() {
		if (waypoints != null && !ai.playerInSight) {
			for (int i = 0; i < waypoints.Length; i ++) {
				Gizmos.color = Color.red;
				Gizmos.DrawCube(waypoints[i], Vector3.one/2);
				
				if (i == 0)
					Gizmos.DrawLine(transform.position, waypoints[i]);
				else
					Gizmos.DrawLine(waypoints[i-1],waypoints[i]);
			}
		}
	}
}
