/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is save and 
 * load the game option setting
 */
[System.Serializable]
public class OptionFile {
	float volume_music = -20f;
	float volume_sfx;
	float mouseSensitivity = 2f;

	// Save the option setting
	public void SaveData () {
		volume_music = GameMaster.volume_music;
		volume_sfx = GameMaster.volume_sfx;
		mouseSensitivity = GameMaster.mouseSensitivity;
	}
	
	// Load the option setting
	public void LoadData () {
		GameMaster.volume_music = volume_music;
		GameMaster.volume_sfx = volume_sfx;
		GameMaster.mouseSensitivity = mouseSensitivity;
	}
}
