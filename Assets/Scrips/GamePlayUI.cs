/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to controll the game play UI
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GamePlayUI : MonoBehaviour {
	/*
	 * Properties of the UI
	 */
	public Text score;
	public Slider healthSlider;
	public Text time;
    public CursorLockMode wantedMode;
	public Canvas pausedUI;
	public CanvasGroup pausedUICanvasGroup;
	public GameObject weapon;
	public AudioMixerSnapshot pausedAudio;
	public AudioMixerSnapshot unpausedAudio;
	FirstPersonController fpc;

	// Use this for initialization
	void Start () {
		pausedUI.enabled = false;
		pausedUICanvasGroup.interactable = false;
		pausedUICanvasGroup.blocksRaycasts = false;
		fpc = GameObject.Find ("_Player").GetComponent <FirstPersonController> ();
		if(GameMaster.newPlayer){
			Resume();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Update the status value
		score.text = "Score: "+GameMaster.currentScore;
		healthSlider.value = PlayerHealth.currentHealth;
		time.text = GameMaster.GetTime (GameMaster.playedTime);
		// hide or show the cursor
		Cursor.lockState = wantedMode;
		Cursor.visible = (CursorLockMode.Locked != wantedMode);
		// Escape key to pause
		if (Input.GetKeyDown(KeyCode.Escape)){
			if(!pausedUI.enabled)
				Resume();
		}
	}
	/*
	 * Pause and unpause the game
	 */
	public void Resume(){
		// Unlock the cursor
		wantedMode = CursorLockMode.Locked == wantedMode ? CursorLockMode.None : CursorLockMode.Locked;
		// Show paused menu
		pausedUI.enabled = !pausedUI.enabled;
		pausedUICanvasGroup.interactable = !pausedUICanvasGroup.interactable;
		pausedUICanvasGroup.blocksRaycasts = !pausedUICanvasGroup.blocksRaycasts;
		// Hide the weapons
		fpc.enabled = !fpc.enabled;
		weapon.SetActive (!weapon.activeSelf);
		Pause ();
	}
	/*
	 * Return back to main menu
	 */
	public void BackToMenu(){
		Pause ();
		Application.LoadLevel("MainMenu");
	}
	/*
	 * Pause the time in the game
	 */
	void Pause(){
		Time.timeScale = Mathf.Approximately(Time.timeScale, 0) ? 1 : 0;
		// Change the music
		if (Mathf.Approximately(Time.timeScale, 0))
			pausedAudio.TransitionTo (0.01f);
		else
			unpausedAudio.TransitionTo (0.01f);
	}
}
