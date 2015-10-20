using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string DIFFICULTY_KEY = "difficulty";
	const string LEVEL_KEY = "level_unlocked_";
	const string GAME_FILE = "game_file";
	const string FILE_1_PLAYER_NAME = "file_1_player_name";
	const string FILE_1_LEVEL_PROGRESS = "file_1_level_progress";
	const string FILE_2_PLAYER_NAME = "file_2_player_name";
	const string FILE_2_LEVEL_PROGRESS = "file_2_level_progress";
	const string FILE_3_PLAYER_NAME = "file_3_player_name";
	const string FILE_3_LEVEL_PROGRESS = "file_3_level_progress";
	const string TO_DELETE_MENU = "to_delete_menu";
	const string SELECT_ITEM = "select_item";
	
	
	public static void SetSelectItem (int inventoryID) {
		PlayerPrefs.SetInt (SELECT_ITEM, inventoryID);
	}
	
	public static int GetSelectItem () {
		return PlayerPrefs.GetInt (SELECT_ITEM);
	}
	
	
	
	public static void SetDeleteEntryPoint (string levelName) {
		PlayerPrefs.SetString (TO_DELETE_MENU, levelName);
	}
	
	public static string GetDeleteEntryPoint () {
		return PlayerPrefs.GetString (TO_DELETE_MENU);
	}
	
	

	public static void SetMasterVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		}
	}
	
	public static float GetMasterVolume () {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}
	
	
	
	public static void SetGameFile (int file) {
		PlayerPrefs.SetInt (GAME_FILE, file);
	}
	
	public static int GetGameFile () {
		return PlayerPrefs.GetInt (GAME_FILE);
	}
	
	
	
	public static void SetFile1PlayerName (string playerName) {
		PlayerPrefs.SetString (FILE_1_PLAYER_NAME, playerName);
	}
	
	public static string GetFile1PlayerName () {
		return PlayerPrefs.GetString (FILE_1_PLAYER_NAME);
	}
	
	public static void SetFile1LevelProgress (int levelProgress) {
		PlayerPrefs.SetInt (FILE_1_LEVEL_PROGRESS, levelProgress);
	}
	
	public static int GetFile1LevelProgress () {
		return PlayerPrefs.GetInt (FILE_1_LEVEL_PROGRESS);
	}
	
	
	public static void SetFile2PlayerName (string playerName) {
		PlayerPrefs.SetString (FILE_2_PLAYER_NAME, playerName);
	}
	
	public static string GetFile2PlayerName () {
		return PlayerPrefs.GetString (FILE_2_PLAYER_NAME);
	}
	
	public static void SetFile2LevelProgress (int levelProgress) {
		PlayerPrefs.SetInt (FILE_2_LEVEL_PROGRESS, levelProgress);
	}
	
	public static int GetFile2LevelProgress () {
		return PlayerPrefs.GetInt (FILE_2_LEVEL_PROGRESS);
	}
	
	
	public static void SetFile3PlayerName (string playerName) {
		PlayerPrefs.SetString (FILE_3_PLAYER_NAME, playerName);
	}
	
	public static string GetFile3PlayerName () {
		return PlayerPrefs.GetString (FILE_3_PLAYER_NAME);
	}
	
	public static void SetFile3LevelProgress (int levelProgress) {
		PlayerPrefs.SetInt (FILE_3_LEVEL_PROGRESS, levelProgress);
	}
	
	public static int GetFile3LevelProgress () {
		return PlayerPrefs.GetInt (FILE_3_LEVEL_PROGRESS);
	}
	
	
	
	public static void UnlockLevel (int level) {
		if (level <= Application.levelCount - 1) {
			PlayerPrefs.SetInt (LEVEL_KEY + level.ToString (), 1);
		}
	}
	
	public static bool IsLevelUnlocked (int level) {
		int levelValue = PlayerPrefs.GetInt (LEVEL_KEY + level.ToString());
		bool isLevelUnlocked = (levelValue == 1);
		
		if (level <= Application.levelCount - 1) {
			return isLevelUnlocked;
		} else {
			return false;
		}
	}
	
	public static void SetDifficulty (float difficulty) {
		if (difficulty >= 1f && difficulty <= 3f){
			PlayerPrefs.SetFloat (DIFFICULTY_KEY, difficulty);
		}
	}
	
	public static float GetDifficulty () {
		return PlayerPrefs.GetFloat (DIFFICULTY_KEY);
	}
	
}



