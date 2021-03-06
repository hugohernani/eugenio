using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*********************/
/* SAVE DATA IN FILE */
/*********************/

public class SaveLoad {

	List<User.UserDoes> userDoesList;
	List<User.UserPlays> userPlaysList;
	static string userPath;

	public SaveLoad (string userName) {
		userDoesList = new List<User.UserDoes> ();
		userPlaysList = new List<User.UserPlays> ();
		SaveLoad.userPath = Application.persistentDataPath + "/" + userName;

		if(!Directory.Exists(userPath)){
			Directory.CreateDirectory (userPath);
		}

	}

	public bool SaveUserDict (Dictionary<string, string> userDict) {
		string path = userPath + "/savedUserDict.gd";
		FileStream file = File.Create(path);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, userDict);
		file.Close();
		return File.Exists(path);
	}

	public Dictionary<string, string> LoadUserDict () {
		if(File.Exists(userPath + "/savedUserDict.gd")) { 
			string path = userPath + "/savedUserDict.gd";
			FileStream file = File.Open(path, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			Dictionary<string, string> userDict = (Dictionary<string, string>)bf.Deserialize(file);
			file.Close();
			return userDict;
		} else 
			return null;
	}

	public void AddUserDoes(User.UserDoes ud) {
		userDoesList.Add(ud);
	}

	public void AddUserPlays(User.UserPlays up){
		userPlaysList.Add (up);
	}

	public bool SaveUserDoes () {
		string path = userPath + "/savedUserDoesList.gd";
		if(userDoesList.Count != 0){
			FileStream file = File.Create(path);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, userDoesList);
			file.Close();
		}
		return File.Exists(path);
	}

	public bool SaveUserPlays ()
	{
		string path = userPath + "/savedUserPlaysList.gd";
		if(userPlaysList.Count != 0){
			FileStream file = File.Create(path);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, userPlaysList);
			file.Close();
		}
		return File.Exists (path);
	}

	public List<User.UserDoes> LoadUserDoesList () {
		string path = userPath + "/savedUserDoesList.gd";
		if(File.Exists(path)) { 
			FileStream file = File.Open(path, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			userDoesList = (List<User.UserDoes>)bf.Deserialize(file);
			file.Close();
			return userDoesList;
		} else 
			return null;
	}

	public List<User.UserPlays> LoadUserPlaysList ()
	{
		string path = userPath + "/savedUserPlaysList.gd";
		if(File.Exists(path)) { 
			FileStream file = File.Open(path, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			userPlaysList = (List<User.UserPlays>)bf.Deserialize(file);
			file.Close();
			return userPlaysList;
		} else 
			return null;
	}

	public bool SavePet (Pet pet) {
		string path = userPath + "/savedPet.gd";
		FileStream file = File.Create(path);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, pet);
		file.Close();
		return File.Exists(path);
	}

	public Pet LoadPet () {
		if(File.Exists(userPath + "/savedPet.gd")) { 
			string path = userPath + "/savedPet.gd";
			FileStream file = File.Open(path, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			Pet pet = (Pet)bf.Deserialize(file);
			file.Close();
			return pet;
		} else 
			return null;
	}

	public bool destroyUserFolder(){
		if(Directory.Exists(userPath)){
			Directory.Delete (userPath, true);
		}
		return (Directory.Exists (userPath));
	}

	public bool destroyUserFolder(string username){
		string path = Application.persistentDataPath + "/" + username;
		if(Directory.Exists(path)){
			Directory.Delete (path, true);
		}
		return (Directory.Exists (path));
	}


}
