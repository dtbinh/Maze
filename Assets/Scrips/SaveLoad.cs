/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to generate and 
 * load the save file
 */
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad{
	public static SaveFile savedGame = new SaveFile ();
	public static OptionFile savedOption = new OptionFile ();
	/*
	 * Generate the save file for game property
	 */
	public static void Save(){
		SaveLoad.savedGame.SaveData ();
		var bf = new BinaryFormatter ();
		var file = File.Create (Application.persistentDataPath+"/savedGame.sav");
		bf.Serialize (file, SaveLoad.savedGame);
		file.Close ();
	}
	/*
	 * Load the save file for game property
	 */
	public static void Load(){
		if(File.Exists (Application.persistentDataPath + "/savedGame.sav")){
			var bf = new BinaryFormatter ();
			var file = File.Open (Application.persistentDataPath + "/savedGame.sav", FileMode.Open);
			SaveLoad.savedGame = (SaveFile)bf.Deserialize (file);
			file.Close ();
		}
	}
	/*
	 * Generate the save file for game option
	 */
	public static void SaveOption(){
		SaveLoad.savedOption.SaveData ();
		var bf = new BinaryFormatter ();
		var file = File.Create (Application.persistentDataPath+"/option.opt");
		bf.Serialize (file, SaveLoad.savedOption);
		file.Close ();
	}
	/*
	 * Load the save file for game option
	 */
	public static void LoadOption(){
		if(File.Exists (Application.persistentDataPath + "/option.opt")){
			var bf = new BinaryFormatter ();
			var file = File.Open (Application.persistentDataPath + "/option.opt", FileMode.Open);
			SaveLoad.savedOption = (OptionFile)bf.Deserialize (file);
			file.Close ();
		}
	}
}