/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to generate the pick ups
 */
using UnityEngine;
using System.Collections.Generic;

public class PickupGenerator : MonoBehaviour {
	/*
	 * Pick up prefabs
	 */
	public GameObject powerDrop;
	public GameObject speedDrop;
	public GameObject potionDrop;
	public GameObject player;
	public GameObject treasure;
	// The duration of the pick ups
	public float duration;
	/*
	 * Maximum number of pick ups
	 */
	int maxPowerDrops;
	int maxSpeedDrops;
	int maxPotionDrops;
	/*
	 * Current number of pick ups
	 */
	int curNumOfPowerDrops;
	int curNumOfSpeedDrops;
	int curNumOfPotionDrops;
	/*
	 * Location storages
	 */
	List<Vector3> freePos;
	List<Vector3> usedPos;

	// Use this for initialization
	void Start () {
		maxPowerDrops = GameMaster.maxPowerDrops;
		maxSpeedDrops = GameMaster.maxSpeedDrops;
		maxPotionDrops = GameMaster.maxPotionDrops;

		freePos = BlockGridMap.gridMapWorldPosition;

		int index = Random.Range (0, freePos.Count - 1);
		player.transform.position = freePos[index];

		InstantiateTreasure (index);

		usedPos = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
		// Generate power up
		if(curNumOfPowerDrops >= 0 && curNumOfPowerDrops <= maxPowerDrops){
			curNumOfPowerDrops++;
			int index = Random.Range(0,freePos.Count-1);
			usedPos.Add(freePos[index]);
			Destroy (Instantiate(powerDrop, freePos[index],Quaternion.identity), duration);
			freePos.RemoveAt(index);
		}
		// Generate speed up
		if(curNumOfSpeedDrops >= 0 && curNumOfSpeedDrops <= maxSpeedDrops){
			curNumOfSpeedDrops++;
			int index = Random.Range(0,freePos.Count-1);
			usedPos.Add(freePos[index]);
			Destroy (Instantiate(speedDrop, freePos[index],Quaternion.identity), duration);
			freePos.RemoveAt(index);
		}
		// Generate potion
		if(curNumOfPotionDrops >= 0 && curNumOfPotionDrops <= maxPotionDrops){
			curNumOfPotionDrops++;
			int index = Random.Range(0,freePos.Count-1);
			usedPos.Add(freePos[index]);
			Destroy (Instantiate(potionDrop, freePos[index],Quaternion.identity),duration);
			freePos.RemoveAt(index);
		}
	}
	/*
	 * Generate the treasures in random locations
	 */
	void InstantiateTreasure(int playerIndex){
		int[] treasurePosition = new int[GameMaster.targetTreasureNumber];
		treasurePosition [0] = 5;
		treasurePosition [GameMaster.targetTreasureNumber-1] = GameMaster.worldPosZ - 5;

		for(int i = 1; i < treasurePosition.Length-1; i++){
			treasurePosition [i] = GameMaster.worldPosZ / (i + 1);
		}

		for(int i = 0; i < treasurePosition.Length; i++){
			bool instantiated = false;
			do{
				int index = Random.Range (0, freePos.Count - 1);
				if(freePos[index] != freePos[playerIndex] && Mathf.Approximately (freePos[index].z, treasurePosition[i])){
					Instantiate (treasure, freePos[index], Quaternion.identity);
					instantiated = true;
				}
			}while(!instantiated);
		}
	}
	/* 
	 * Update the map when the pick up is taking
	 * @name: the pick up name
	 * @pos: the location where its taken
	 */
	public void PickupTaken(string name, Vector3 pos){
		switch(name){
		case "Power(Clone)":
			curNumOfPowerDrops--;
			break;
		case "Speed(Clone)":
			curNumOfSpeedDrops--;
			break;
		case "Potion(Clone)":
			curNumOfPotionDrops--;
			break;
		}
		usedPos.Remove(pos);
		freePos.Add(pos);
	}
}