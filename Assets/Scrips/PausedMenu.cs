/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to controll the paused menu UI
 */
using UnityEngine;
using UnityEngine.UI;

public class PausedMenu : MonoBehaviour {
	/*
	 * Paused Menu UI properties
	 */
	public RectTransform menuPanel;
	public CanvasGroup menuCG;
	public CanvasGroup helpCG;
	public CanvasGroup optionCG;
	public CanvasGroup msgBoxCG;
	public GameObject[] panels = new GameObject[3];
	public Text pageNum;
	int index;

	void Start(){
		// Show Help menu if new player
		if(GameMaster.newPlayer){
			OpenSubMenus("Help");
		}
	}
	/*
	 * Open the submenus from the main menu
	 * @submenu: the name of the sub menu
	 */
	public void OpenSubMenus(string subMenu){
		switch(subMenu){
		case"Help":
			helpCG.alpha = 1;
			helpCG.interactable = true;
			helpCG.blocksRaycasts = true;
			break;
		case "Option":
			optionCG.alpha = 1;
			optionCG.interactable = true;
			optionCG.blocksRaycasts = true;
			menuPanel.sizeDelta = new Vector2(936, 500);
			break;
		case"MsgBox":
			msgBoxCG.alpha = 1;
			msgBoxCG.interactable = true;
			msgBoxCG.blocksRaycasts = true;
			break;
		}
		menuCG.alpha = 0;
		menuCG.interactable = false;
		menuCG.blocksRaycasts = false;
	}
	/*
	 * Close the submenus by hide and disable their
	 * interaction
	 */
	public void CloseSubMenus(){
		helpCG.alpha = 0;
		helpCG.interactable = false;
		helpCG.blocksRaycasts = false;
		msgBoxCG.alpha = 0;
		msgBoxCG.interactable = false;
		msgBoxCG.blocksRaycasts = false;
		optionCG.alpha = 0;
		optionCG.interactable = false;
		optionCG.blocksRaycasts = false;
		menuCG.alpha = 1;
		menuCG.interactable = true;
		menuCG.blocksRaycasts = true;
		menuPanel.sizeDelta = new Vector2(450, 500);
		index = 0;
		UpdatePages();
		if(GameMaster.newPlayer)
			GameMaster.newPlayer = false;
	}
	/*
	 * Chnage pages in the help menu
	 */
	public void Prev(){
		index--;
		if(index == -1){
			index = panels.Length-1;
		}
		UpdatePages();
	}
	public void Next(){
		index++;
		if(index == panels.Length){
			index = 0;
		}
		UpdatePages();
	}
	void UpdatePages(){
		foreach(GameObject go in panels){
			go.SetActive(false);
		}
		panels[index].SetActive(true);
		pageNum.text = index+1+"/3";
	}
}
