/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to handle data in the game
 * and transfer dta between them.
 */

using UnityEngine;
using UnityEngine.Audio;

public class GameMaster : MonoBehaviour {
	/*
	 * The audio Manager
	 */
	public AudioMixer audioManager;
	/*
	 * Property for game play data storage
	 */
	public static string currentMode;
	public static int currentScore;
	public static int totalBotKilled;
	public static float playedTime = 6000;
	public static int attackbotKilled;
	public static int speedbotKilled;
	public static int defencebotKilled;
	/*
	 * Property for easy mode data storage
	 */
	public static int easy_attackbotKilled;
	public static int easy_speedbotKilled;
	public static int easy_defencebotKilled;
	public static float easy_topClearTime = 6000;
	public static int easy_gameClears;
	public static int easy_topScore;
	/*
	 * Property for normal mode data storage
	 */
	public static int normal_attackbotKilled;
	public static int normal_speedbotKilled;
	public static int normal_defencebotKilled;
	public static float normal_topClearTime = 6000;
	public static int normal_gameClears;
	public static int normal_topScore;
	/*
	 * Property for hard mode data storage
	 */
	public static int hard_attackbotKilled;
	public static int hard_speedbotKilled;
	public static int hard_defencebotKilled;
	public static float hard_topClearTime = 6000;
	public static int hard_gameClears;
	public static int hard_topScore;
	/*
	 * Property for initiate new game
	 */
	public static int gridSizeOffset = 10;
	public static int gridSizeX;
	public static int gridSizeY;
	public static int gridSizeZ;
	public static int worldPosX;
	public static int worldPosY;
	public static int worldPosZ;
	public static int maxNumberOfAttackBots;
	public static int maxNumberOfDefenceBots;
	public static int maxNumberOfSpeedBots;
	public static int maxAddNumOfAtkBots;
	public static int maxAddNumOfDefBots;
	public static int maxAddNumOfSpdBots;
	public static int maxPowerDrops;
	public static int maxSpeedDrops;
	public static int maxPotionDrops;
	public static bool win;
	public static int targetTreasureNumber = 3;
	public static int botPowerUpgrade;
	/*
	 * Property for option menu
	 */
	public static float volume_sfx;
	public static float volume_music;
	public static float mouseSensitivity;
	/*
	 * Property for game saving
	 */
	public static bool newPlayer = true;
	static bool created;

	// Use this for initialization
	void Awake () {
		// Keep this game object in the scene
		DontDestroyOnLoad(gameObject);
		// Load saved files
		if (!created) {
			created = true;	
			SaveLoad.Load ();
			SaveLoad.savedGame.LoadData ();
			SaveLoad.LoadOption ();
			SaveLoad.savedOption.LoadData ();
		}
		else
			DestroyImmediate(gameObject);
		// Set the audio volume
		audioManager.SetFloat ("musicVol", volume_music);
		audioManager.SetFloat ("sfxVol", volume_sfx);
	}

	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName == "Level_GamePlay"){
			playedTime = Time.timeSinceLevelLoad;
		}
	}

	/*
	 * Conver time from seconds to hh:mm:ss format
	 */
	public static string  GetTime(float time){
		int minutes = Mathf.FloorToInt(time / 60f);
		int seconds = Mathf.FloorToInt(time - minutes * 60f);
		return string.Format("{0:0}:{1:00}", minutes, seconds);
	}

	/*
	 * Select a game mode and specify the properties
	 */
	public static void InitGame(string difficulty){
		switch(difficulty){
		case "Easy":
			currentMode = "Easy";
			gridSizeX = 5;
			gridSizeY = 2;
			gridSizeZ = 5;
			maxNumberOfAttackBots = 7;
			maxNumberOfDefenceBots = 3;
			maxNumberOfSpeedBots = 1;
			maxAddNumOfAtkBots = 5;
        	maxAddNumOfDefBots = 2;
        	maxAddNumOfSpdBots = 0;
			maxPowerDrops = 2;
			maxSpeedDrops = 2;
			maxPotionDrops = 3;
			botPowerUpgrade = 0;
			break;
		case "Normal":
			currentMode = "Normal";
			gridSizeX = 5;
			gridSizeY = 3;
			gridSizeZ = 5;
			maxNumberOfAttackBots = 15;
			maxNumberOfDefenceBots = 5;
			maxNumberOfSpeedBots = 3;
			maxAddNumOfAtkBots = 10;
			maxAddNumOfDefBots = 5;
			maxAddNumOfSpdBots = 2;
			maxPowerDrops = 3;
			maxSpeedDrops = 3;
			maxPotionDrops = 6;
			botPowerUpgrade = 1;
			break;
		case "Hard":
			currentMode = "Hard";
			gridSizeX = 5;
			gridSizeY = 5;
			gridSizeZ = 5;
			maxNumberOfAttackBots = 30;
			maxNumberOfDefenceBots = 20;
			maxNumberOfSpeedBots = 10;
			maxAddNumOfAtkBots = 15;
			maxAddNumOfDefBots = 10;
			maxAddNumOfSpdBots = 5;
			maxPowerDrops = 5;
			maxSpeedDrops = 5;
			maxPotionDrops = 8;
			botPowerUpgrade = 2;
			break;
		}

		worldPosX = gridSizeX * 10;
		worldPosY = gridSizeY * 10;
		worldPosZ = gridSizeZ * 10;
	}

	/*
	 * Update the player profile
	 */
	public static void UpdateProfile(){
		switch(currentMode){
		case "Easy":
			easy_attackbotKilled += attackbotKilled;
			easy_speedbotKilled += speedbotKilled;
			easy_defencebotKilled += defencebotKilled;
			// Top score and number of clear game only update if the player wins
			if (win) {
				easy_topScore = easy_topScore < currentScore ? currentScore : easy_topScore;
				easy_gameClears++;
				if (easy_topClearTime > playedTime)
					easy_topClearTime = playedTime;
			}
			break;
		case "Normal":
			normal_attackbotKilled += attackbotKilled;
			normal_speedbotKilled += speedbotKilled;
			normal_defencebotKilled += defencebotKilled;
			if (win) {
				normal_topScore = normal_topScore < currentScore ? currentScore : normal_topScore;
				normal_gameClears++;
				if (normal_topClearTime > playedTime)
					normal_topClearTime = playedTime;
			}
			break;
		case "Hard":
			hard_attackbotKilled += attackbotKilled;
			hard_speedbotKilled += speedbotKilled;
			hard_defencebotKilled += defencebotKilled;
			if (win) {
				hard_topScore = hard_topScore < currentScore ? currentScore : hard_topScore;
				hard_gameClears++;
				if (hard_topClearTime > playedTime)
					hard_topClearTime = playedTime;
			}
			break;
		}
		// The data storage should reset after game finish
		attackbotKilled = 0;
    	speedbotKilled = 0;
    	defencebotKilled = 0;
		currentScore = 0;
		totalBotKilled = 0;
	}
}
