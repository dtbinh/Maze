/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to controll the option menu UI
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour {
	/*
	 * The properties of the UI
	 */
	public Slider sfx;
	public Slider music;
	public Slider mouseSensitivity;
	AudioMixer audioMixer;

	// Use this for initialization
	void Start () {
		audioMixer = GameObject.Find ("_GameMaster").GetComponent <GameMaster> ().audioManager;
		sfx.value = GameMaster.volume_sfx;
		music.value = GameMaster.volume_music;
		mouseSensitivity.value = GameMaster.mouseSensitivity;
	}
	/*
	 * Set the game music volume
	 * @vol: the number for the volume
	 */
	public void SetMusicVolume(float vol){
		audioMixer.SetFloat ("musicVol", vol);
		GameMaster.volume_music = vol;
	}
	/*
	 * Set the sound effect
	 * @vol: the number for the volume
	 */
	public void SetSFXVolume(float vol){
		audioMixer.SetFloat ("sfxVol", vol);
		GameMaster.volume_sfx = vol;
	}
	/*
	 * Set the mouse sensitivity
	 * @speed: the speed value
	 */
	public void SetMouseSensitivity(float speed){
		GameMaster.mouseSensitivity = speed;
	}
	/*
	 * Reset the setting to the default value
	 */
	public void SetDefaultSetting(){
		sfx.value = GameMaster.volume_sfx = 0;
		music.value = GameMaster.volume_music = -20f;
		audioMixer.SetFloat ("musicVol", GameMaster.volume_music);
		audioMixer.SetFloat ("sfxVol", GameMaster.volume_sfx);
		mouseSensitivity.value = GameMaster.mouseSensitivity = 2f; 
	}
	/*
	 * Save the option setting
	 */
	public void SaveOption(){
		SaveLoad.SaveOption ();
	}
}
