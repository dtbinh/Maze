/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is save and 
 * load the game profile
 */
[System.Serializable]
public class SaveFile {
	/*
	 * Property for easy mode
	 */
	int easy_attackbotKilled;
	int easy_speedbotKilled;
	int easy_defencebotKilled;
	float easy_topClearTime = 6000;
	int easy_gameClears;
	int easy_topScore;
	/*
	 * Property for normal mode
	 */
	int normal_attackbotKilled;
	int normal_speedbotKilled;
	int normal_defencebotKilled;
	float normal_topClearTime = 6000;
	int normal_gameClears;
	int normal_topScore;
	/*
	 * Property for hard mode
	 */
	int hard_attackbotKilled;
	int hard_speedbotKilled;
	int hard_defencebotKilled;
	float hard_topClearTime = 6000;
	int hard_gameClears;
	int hard_topScore;
	// New / old player
	bool newPlayer = true;
	/*
	 * Save the game data
	 */
	public void SaveData(){
		easy_attackbotKilled = GameMaster.easy_attackbotKilled;
		easy_speedbotKilled = GameMaster.easy_speedbotKilled;
		easy_defencebotKilled = GameMaster.easy_defencebotKilled;
		easy_topClearTime = GameMaster.easy_topClearTime;
		easy_gameClears = GameMaster.easy_gameClears;
		easy_topScore = GameMaster.easy_topScore;

		normal_attackbotKilled = GameMaster.normal_attackbotKilled;
		normal_speedbotKilled = GameMaster.normal_speedbotKilled;
		normal_defencebotKilled = GameMaster.normal_defencebotKilled;
		normal_topClearTime = GameMaster.normal_topClearTime;
		normal_gameClears = GameMaster.normal_gameClears;
		normal_topScore = GameMaster.normal_topScore;

		hard_attackbotKilled = GameMaster.hard_attackbotKilled;
		hard_speedbotKilled = GameMaster.hard_speedbotKilled;
		hard_defencebotKilled = GameMaster.hard_defencebotKilled;
		hard_topClearTime = GameMaster.hard_topClearTime;
		hard_gameClears = GameMaster.hard_gameClears;
		hard_topScore = GameMaster.hard_topScore;

		newPlayer = GameMaster.newPlayer;
	}
	/*
	 * Load the gaem data
	 */
	public void LoadData(){
		GameMaster.easy_attackbotKilled = easy_attackbotKilled;
		GameMaster.easy_speedbotKilled = easy_speedbotKilled;
		GameMaster.easy_defencebotKilled = easy_defencebotKilled;
		GameMaster.easy_topClearTime = easy_topClearTime;
		GameMaster.easy_gameClears = easy_gameClears;
		GameMaster.easy_topScore = easy_topScore;

		GameMaster.normal_attackbotKilled = normal_attackbotKilled;
		GameMaster.normal_speedbotKilled = normal_speedbotKilled;
		GameMaster.normal_defencebotKilled = normal_defencebotKilled;
		GameMaster.normal_topClearTime = normal_topClearTime;
		GameMaster.normal_gameClears = normal_gameClears;
		GameMaster.normal_topScore = normal_topScore;

		GameMaster.hard_attackbotKilled = hard_attackbotKilled;
		GameMaster.hard_speedbotKilled = hard_speedbotKilled;
		GameMaster.hard_defencebotKilled = hard_defencebotKilled;
		GameMaster.hard_topClearTime = hard_topClearTime;
		GameMaster.hard_gameClears = hard_gameClears;
		GameMaster.hard_topScore = hard_topScore;

		GameMaster.newPlayer = newPlayer;
	}
}
