using UnityEngine;
using System.Collections;

public class BlockNode{
	public int code;
	public Vector3 worldPosition;
	public int x;
	public int y;
	public int z;
	
	public BlockNode(int code, Vector3 worldPosition, int x, int y, int z){
		this.code = code;
		this.worldPosition = worldPosition;
		this.x = x;
		this.y = y;
		this.z = z;
	}
}
