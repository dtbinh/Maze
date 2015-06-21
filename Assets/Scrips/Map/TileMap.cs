using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {
	public TileType[] tileTypes;
	public static int[,,] gridMap;
	public bool showTop;
	int offset;
	int gridSizeX;
	int gridSizeY;
	int gridSizeZ;

	// Use this for initialization
	void Awake () {
		gridSizeX = GameMaster.gridSizeX;
		gridSizeY = GameMaster.gridSizeY;
		gridSizeZ = GameMaster.gridSizeZ;
		offset = GameMaster.gridSizeOffset;
		GenerateMapData();
		GenerateMapVisual();
	}

	void GenerateMapData(){
		gridMap = new int[gridSizeX, gridSizeY, gridSizeZ];
		Generate(0,0,0);
	}

	void GenerateMapVisual(){
		//TileType large_ground = tileTypes[0];
		TileType horizontal_wall = tileTypes[1];
		TileType vertical_wall = tileTypes[2];
		TileType small_wall = tileTypes[3];
		TileType ground = tileTypes[0];
		TileType empty_ground = tileTypes[4];

		GameObject t;

		for(int y = 0; y < gridSizeY; y++){
			// draw Wall from north to south
			for (int z = gridSizeZ; 0 < z; z--) {
				for (int x = 0; x < gridSizeX; x++) {
					// draw the north wall
					if((gridMap[x,y,z-1] & 1) == 0)
						t = (GameObject)Instantiate(horizontal_wall.tileVisualPrefab, new Vector3(x*offset+4.5f, y*offset-.5f, z*offset), Quaternion.identity);
					else
						t = (GameObject)Instantiate(small_wall.tileVisualPrefab, new Vector3(x*offset, y*offset-.5f, z*offset), Quaternion.identity);
					t.gameObject.transform.parent = transform;
					// draw the west wall
					if((gridMap[x,y,z-1] & 8) == 0)
						t = (GameObject)Instantiate(vertical_wall.tileVisualPrefab, new Vector3(x*offset, y*offset-.5f, z*offset-5), Quaternion.identity);
					t.gameObject.transform.parent = transform;
					// draw the down ground
					if((gridMap[x,y,z-1] & 32) == 0)
						t = (GameObject)Instantiate(ground.tileVisualPrefab, new Vector3(x*offset+5f, y*offset-.5f, z*offset-5f), Quaternion.identity);
					else
						t = (GameObject)Instantiate(empty_ground.tileVisualPrefab, new Vector3(x*offset+5f, y*offset-.5f, z*offset-5f), Quaternion.identity);
					t.gameObject.transform.parent = transform;
				}
				t = (GameObject)Instantiate(small_wall.tileVisualPrefab, new Vector3(gridSizeX*offset, y*offset-.5f, z*offset), Quaternion.identity);
				t.gameObject.transform.parent = transform;
				t = (GameObject)Instantiate(vertical_wall.tileVisualPrefab, new Vector3(gridSizeX*offset, y*offset-.5f, z*offset-5), Quaternion.identity);
				t.gameObject.transform.parent = transform;
			}
			// draw the south wall
			for (int x = 0; x < gridSizeX; x++){
				t = (GameObject)Instantiate(horizontal_wall.tileVisualPrefab, new Vector3(x*offset+4.5f, y*offset-.5f, 0), Quaternion.identity);
				t.gameObject.transform.parent = transform;
			}

			t = (GameObject)Instantiate(small_wall.tileVisualPrefab, new Vector3(gridSizeX*offset, y*offset-.5f, 0), Quaternion.identity);
			t.gameObject.transform.parent = transform;
		}

		// Draw roft
		if(showTop){
			for (int z = gridSizeZ; 0 < z; z--) {
				for (int x = 0; x < gridSizeX; x++) {
					// draw the down ground
					if((gridMap[x,gridSizeY-1,z-1] & 16) == 0){
						t = (GameObject)Instantiate(ground.tileVisualPrefab, new Vector3(x*offset+5f, gridSizeY*offset-.5f, z*offset-5f), Quaternion.identity);
						t.gameObject.transform.parent = transform;
					}
				}
			}
		}
	}

	void Generate (int cx, int cy, int cz){
		Direction[] direction = new Direction[6]{new Direction("North"), new Direction("South"), 
			new Direction("West"), new Direction("East"), new Direction("Up"), new Direction("Down")};
		Shuffle(direction);
		for(int i = 0; i < direction.Length; i++){
			int nx = cx + direction[i].x;
			int ny = cy + direction[i].y;
			int nz = cz + direction[i].z;
			if(Between(nx, gridSizeX) && Between(ny, gridSizeY) && Between(nz, gridSizeZ) && (gridMap[nx,ny,nz] == 0)) {
				gridMap[cx,cy,cz] |= direction[i].bit;
				gridMap[nx,ny,nz] |= direction[i].oBit;
				Generate(nx,ny,nz);
			}
		}
	}
	
	bool Between(int val, int max) {
		return (val >= 0) && (val < max);
	}
	
	void Shuffle(Direction[] array) {
		int m = array.Length;
		// While there remain elements to Shuffle…
		for(int j = 0; j <= m; j++) {
			// Pick a remaining element…
			int i = (int)Mathf.Floor(Random.value * m--);
			// And swap it with the current element.
			Direction t = array[m];
			array[m] = array[i];
			array[i] = t;
		}
	}
	
	class Direction{
		public int bit;
		public int oBit;
		public int x, y, z;
		public string s;
		
		public Direction(string s){
			this.s = s;
			setDirection();
		}
		
		public void setDirection(){
			switch(s){
			case"North": 
				bit = 1;
				oBit = 2;
				x = 0;
				y = 0;
				z = 1;
				break;
			case"South":
				bit = 2;
				oBit = 1;
				x = 0;
				y = 0;
				z = -1;
				break;
			case"East":
				bit = 4;
				oBit = 8;
				x = 1;
				y = 0;
				z = 0;
				break;
			case"West":
				bit = 8;
				oBit = 4;
				x = -1;
				y = 0;
				z = 0;
				break;
			case"Up":
				bit = 16;
				oBit = 32;
				x = 0;
				y = 1;
				z = 0;
				break;
			case"Down":
				bit = 32;
				oBit = 16;
				x = 0;
				y = -1;
				z = 0;
				break;
			}
		}
	}
}
