/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to controll the resul menu UI
 */
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour {
	/*
	 * The result menu properties
	 */
	public Text header;
	public Text score;
	public Text botsDefeated;
	public Text time;
	public AudioSource win_music;
	public AudioSource lost_music;
	public Animation anim;
	public AudioSource hitSound;

	// Use this for initialization
	void Start () {
		// Check if player win or lose
		if (GameMaster.win){
			win_music.Play ();
			GameMaster.currentScore *= 3;
		}
		else
			lost_music.Play ();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		score.text = ""+GameMaster.currentScore;
		botsDefeated.text = ""+GameMaster.totalBotKilled;
		time.text = GameMaster.GetTime (GameMaster.playedTime);
		header.text = GameMaster.win ? "Congratulations!" : "Defeated!";
		Invoke("DisplayScore",.5f);
		GameMaster.UpdateProfile ();
		SaveLoad.Save ();
	}
	/*
	 * Distaply the score value in the UI
	 */
	void DisplayScore(){
		hitSound.Play();
		anim.Play("DisplayScore");
		Invoke("DisplayBot", .5f);
	}
	/*
	 * Distaply the bot defeated value in the UI
	 */
	void DisplayBot(){
		hitSound.Play();
		anim.Play("DisplayBot");
		Invoke("DisplayTime", .5f);
	}
	/*
	 * Distaply the time value in the UI
	 */
	void DisplayTime(){
		hitSound.Play();
		anim.Play("DisplayTime");
	}
	/*
	 * Return bakc to main menu
	 */
	public void BackToMenu(){
		Application.LoadLevel("MainMenu");
	}
}
