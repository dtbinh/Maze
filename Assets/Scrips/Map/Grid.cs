using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	public static List<Vector3> walkablePos;
	public LayerMask unwalkableMask;
	float nodeRadius = 0.5f;
	Node[,,] grid;
	int gridSizeX, gridSizeY, gridSizeZ;
	int gridWorldSizeX, gridWorldSizeY, gridWorldSizeZ;
	int offset;
	
	void Awake() {
		gridWorldSizeX = GameMaster.gridSizeX * GameMaster.gridSizeOffset;
		gridWorldSizeY = GameMaster.gridSizeY * GameMaster.gridSizeOffset;
		gridWorldSizeZ = GameMaster.gridSizeZ * GameMaster.gridSizeOffset;
		transform.Translate(new Vector3(gridWorldSizeX/2, 0, gridWorldSizeZ/2));
		gridSizeX = Mathf.RoundToInt(gridWorldSizeX);
		gridSizeY = Mathf.RoundToInt(gridWorldSizeY);
		gridSizeZ = Mathf.RoundToInt(gridWorldSizeZ);
		CreateGrid();

	}

	public int MaxSize{
		get{
			return gridSizeX * gridSizeY * gridSizeZ;
		}
	}
	
	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY,gridSizeZ];
		walkablePos = new List<Vector3>();
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSizeX/2 - Vector3.forward * gridWorldSizeZ/2;

		for (int y = 0; y < gridSizeY; y++) {
			for (int x = 0; x < gridSizeX; x++) {
				for (int z = 0; z < gridSizeZ; z++) {
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + nodeRadius) + Vector3.forward * (z + nodeRadius) + Vector3.up * (y + nodeRadius);
					bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius+0,unwalkableMask));
					grid[x,y,z] = new Node(walkable,worldPoint,x,y,z);
					if(walkable)
						walkablePos.Add(worldPoint);
				}
			}
		}
	}
	
	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		/*for(int y = -1; y <= 1; y++){
			for (int x = -1; x <= 1; x++) {
				for (int z = -1; z <= 1; z++) {
					if (x == 0 && z == 0 && y == 0)
						continue;
					
					int checkX = node.gridX + x;
					int checkY = node.gridY + y;
					int checkZ = node.gridZ + z;
					
					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ) {
						neighbours.Add(grid[checkX,checkY,checkZ]);
					}
				}
			}
		}*/
		neighbours.Add(grid[node.gridX,node.gridY-1<0?0:node.gridY-1,node.gridZ]);
		neighbours.Add(grid[node.gridX,node.gridY+1<gridSizeY?node.gridY+1:gridSizeY-1,node.gridZ]);
		neighbours.Add(grid[node.gridX-1<0?0:node.gridX-1,node.gridY,node.gridZ]);
		neighbours.Add(grid[node.gridX+1<gridSizeX?node.gridX+1:gridSizeX-1,node.gridY,node.gridZ]);
		neighbours.Add(grid[node.gridX,node.gridY,node.gridZ-1<0?0:node.gridZ-1]);
		neighbours.Add(grid[node.gridX,node.gridY,node.gridZ+1<gridSizeZ?node.gridZ+1:gridSizeZ-1]);
		
		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPos) {
		int x = (int)worldPos.x;
		int y = (int)worldPos.y;
		int z = (int)worldPos.z;
		x = (x<0)?0:x;
		y = (y<0)?0:y;
		z = (z<0)?0:z;
		x = (x>gridWorldSizeX)?gridWorldSizeX:x;
		y = (y>gridWorldSizeY)?gridWorldSizeY:y;
		z =	(z>gridWorldSizeZ)?gridWorldSizeZ:z;
		return grid[x,y,z];
	}

	public Vector3[] path;

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position+Vector3.up*gridSizeY/2 ,new Vector3(gridWorldSizeX,gridWorldSizeY,gridWorldSizeZ));
		if (grid != null) {
			/*if (path != null) {
				foreach (Vector3 v in path) {
					Gizmos.color = Color.black;
					Gizmos.DrawCube(v, Vector3.one * (.5f));
				}
			}*/
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (.5f));
			}
			/*for (int y = 0; y < gridSizeY; y++) {
				for (int x = 0; x < gridSizeX; x++) {
					for (int z = 0; z < gridSizeZ; z++) {
						Gizmos.color = (grid[x,y,z].walkable)?Color.white:Color.red;
						Gizmos.DrawCube(grid[x,y,z].worldPosition, Vector3.one * (.5f));
					}
				}
			}*/
		}
	}
}
