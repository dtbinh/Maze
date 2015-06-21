using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public Menu CurrentMenu;
	public MovieTexture guide;
	public RawImage video;

	void Start(){
		ShowMenu(CurrentMenu);
		video.texture = guide;
	}

	public void ShowMenu(Menu menu){
		if(CurrentMenu != null)
			CurrentMenu.IsOpen = false;

		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	}

	public void StartGame(string difficulty){
		GameMaster.InitGame (difficulty);
		Application.LoadLevel ("Level_GamePlay");
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void PlayPauseTutorial(){
		if (guide.isPlaying) {
			guide.Pause();
		}
		else {
			guide.Play();
		}
	}

	public void StopTutorial(){
		guide.Stop();
	}
}
