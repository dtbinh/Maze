using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockGridMap : MonoBehaviour {
	public static BlockNode[,,] grid;
	int gridSizeX;
	int gridSizeY;
	int gridSizeZ;
	int offset;
	public static List<Vector3> gridMapWorldPosition;
	// Use this for initialization
	void Awake () {
		gridSizeX = GameMaster.gridSizeX;
		gridSizeY = GameMaster.gridSizeY;
		gridSizeZ = GameMaster.gridSizeZ;
		offset = GameMaster.gridSizeOffset;
		GenerateBlockData();
	}

	void GenerateBlockData(){
		grid = new BlockNode[gridSizeX,gridSizeY,gridSizeZ];
		gridMapWorldPosition = new List<Vector3>();
		for (int y = 0; y < gridSizeY; y++) {
			for (int z = 0; z < gridSizeZ; z++) {
				for (int x = 0; x < gridSizeX; x++) {
					var v = new Vector3(x*offset+5f,y*offset+5f,(gridSizeZ-z)*offset-5f);
					grid[x,y,z] = new BlockNode(TileMap.gridMap[x,y,gridSizeX-1-z],v,x,y,z);
					gridMapWorldPosition.Add(v);
				}
			}
		}
	}

	public List<BlockNode> GetNeighbours(Vector3 pos) {
		BlockNode node = GetBlock(pos);
		var neighbours = new List<BlockNode>();
		// North wall
		if(((node.code & 2) != 0) && (node.z+1 < gridSizeZ))
			neighbours.Add(grid[node.x,node.y,node.z+1]);
		// South wall
		if(((node.code & 1) != 0) && (node.z-1 >= 0))
			neighbours.Add(grid[node.x,node.y,node.z-1]);
		// East wall
		if(((node.code & 4) != 0) && (node.x+1 < gridSizeX))
			neighbours.Add(grid[node.x+1,node.y,node.z]);
		// West wall
		if(((node.code & 8) != 0) && (node.x-1 >= 0))
			neighbours.Add(grid[node.x-1,node.y,node.z]);
		// Up wall
		if(((node.code & 16) != 0) && (node.y+1 < gridSizeY))
			neighbours.Add(grid[node.x,node.y+1,node.z]);
		// Down wall
		if(((node.code & 32) != 0) && (node.y-1 >= 0))
			neighbours.Add(grid[node.x,node.y-1,node.z]);
		return neighbours;
	}

	public BlockNode GetBlock(Vector3 pos){
		int x = (int)pos.x/offset;
		int y = (int)pos.y/offset;
		int z = (int)pos.z/offset;

		x = (x<0)?0:x;
		y = (y<0)?0:y;
		z = (z<0)?0:z;
		x = (x>=gridSizeX)?gridSizeX-1:x;
		y = (y>=gridSizeY)?gridSizeY-1:y;
		z =	(z>=gridSizeZ)?gridSizeZ-1:z;

		return grid[Mathf.FloorToInt(x),Mathf.FloorToInt(y),Mathf.FloorToInt((gridSizeZ-1)-z)];
	}

	public bool CompareBlocks(Vector3 a, Vector3 b){
		return object.Equals (GetBlock (a).worldPosition, GetBlock (b).worldPosition);
	}

	void OnDrawGizmos() {
		if (grid != null) {
			foreach(BlockNode bn in grid){
				if(GetNeighbours(bn.worldPosition).Count == 0){
					Gizmos.color = Color.red;
					Gizmos.DrawCube(bn.worldPosition, Vector3.one);
				}else{
					Gizmos.color = Color.green;
					Gizmos.DrawCube(bn.worldPosition, Vector3.one);
				}

				foreach(BlockNode nei in GetNeighbours(bn.worldPosition)){
					Gizmos.color = Color.green;
					Gizmos.DrawLine(bn.worldPosition, nei.worldPosition);
				}
			}
		}
	}
}
