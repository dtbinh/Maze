using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {
	public int buffTime;
	public static int damageIncreased;
	public int speedIncreased = 3;
	public Image buffIcon;
	public Text treasureDisplay;
	public AudioSource treasureOpen;
	public AudioClip winClip;

	int treasureCounter;

	FirstPersonController fpController;

	public static byte status;

	// Use this for initialization
	void Start () {
		damageIncreased = 0;
		status = 0;
		fpController = GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
		treasureDisplay.text = treasureCounter+"/3";
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Chest"){
			treasureCounter++;
			c.GetComponent<BoxCollider> ().enabled = false;
			treasureOpen.Play ();
			Destroy (c.gameObject, 1);
			if(treasureCounter == GameMaster.targetTreasureNumber){
				PlayerHealth.isAlive = false;
				treasureOpen.clip = winClip;
				treasureOpen.Play();
				Invoke ("Load", 3f);
			}
		}
	}

	void Load(){
		GameMaster.win = true;
		Application.LoadLevel("Level_Result");
	}

	public void Buff(string name){
		ResetStats();

		switch(name){
		case "Power(Clone)":
			damageIncreased = 10;
			buffIcon.color = Color.red;
			status = 1;
			break;
		case "Speed(Clone)":
			fpController.currentSpeed = fpController.baseSpeed + speedIncreased;
			buffIcon.color = Color.blue;
			status = 2;
			break;
		}
		CancelInvoke("ResetStats");
		Invoke("ResetStats", buffTime);
	}

	public void ResetStats(){
		damageIncreased = 0;
		fpController.currentSpeed = fpController.baseSpeed;
		status = 0;
		buffIcon.color = Color.clear;
	}
}
