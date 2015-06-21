using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour {
	public Animation Self;
	public Animation Other;
	public Animation Other2;

	public Text attackbotKilled;
	public Text speedbotKilled;
	public Text defencebotKilled;
	public Text topClearTime;
	public Text gameClears;
	public Text topScore;

	void Start(){
		switch(name){
		case "Panel":
			attackbotKilled.text = GameMaster.easy_attackbotKilled+"";
			speedbotKilled.text = GameMaster.easy_speedbotKilled+"";
			defencebotKilled.text = GameMaster.easy_defencebotKilled+"";
			topClearTime.text = GameMaster.GetTime (GameMaster.easy_topClearTime);
			gameClears.text = GameMaster.easy_gameClears+"";
			topScore.text = GameMaster.easy_topScore+"";
			break;
		case "Normal":
			attackbotKilled.text = GameMaster.normal_attackbotKilled+"";
			speedbotKilled.text = GameMaster.normal_speedbotKilled+"";
			defencebotKilled.text = GameMaster.normal_defencebotKilled+"";
			topClearTime.text = GameMaster.GetTime (GameMaster.normal_topClearTime);
			gameClears.text = GameMaster.normal_gameClears+"";
			topScore.text = GameMaster.normal_topScore+"";
			break;
		case "Hard":
			attackbotKilled.text = GameMaster.hard_attackbotKilled+"";
			speedbotKilled.text = GameMaster.hard_speedbotKilled+"";
			defencebotKilled.text = GameMaster.hard_defencebotKilled+"";
			topClearTime.text = GameMaster.GetTime(GameMaster.hard_topClearTime);
			gameClears.text = GameMaster.hard_gameClears+"";
			topScore.text = GameMaster.hard_topScore+"";
			break;
		}
	}

	public void EasyToNormal(){
		Self.Play ("EasyClosed");
		Other.Play ("EasyToNormal");
	}

	public void NormalToHard(){
		Self.Play ("NormalToHard");
		Other2.Play ("HardOpen");
	}

	public void NormalToEasy(){
		Self.Play ("NormalToEasy");
		Other.Play ("EasyOpen");
	}

	public void HardToNormal(){
		Self.Play ("HardClosed");
		Other.Play ("HardToNormal");
	}
}
