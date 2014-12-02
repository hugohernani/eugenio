using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*********************/
/* SAVE DATA IN FILE */
/*********************/

public class SaveLoad {

	List<User.UserDoes> userDoesList;
	
	string userPath;

	public SaveLoad (string userName) {
        userDoesList = new List<User.UserDoes>();
		userPath = Application.persistentDataPath + "/" + userName;
	}

	public void SaveUserDict (Dictionary<string, string> userDict) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(userPath + "/savedUserDict.gd");
		bf.Serialize(file, userDict);
		file.Close();
	}

	public Dictionary<string, string> LoadUserDict () {
		if(File.Exists(userPath + "/savedUserDict.gd")) { 
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(userPath + "/savedUserDict.gd", FileMode.Open);
			Dictionary<string, string> userDict = (Dictionary<string, string>)bf.Deserialize(file);
			file.Close();
			
			return userDict;
		} else 
			return null;
	}

	public void SaveUserDoes (User.UserDoes userDoes) {
		userDoesList.Add(userDoes);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(userPath + "/savedUserDoesList.gd");
		bf.Serialize(file, userDoesList);
		file.Close();
	}

	public List<User.UserDoes> LoadUserDoesList () {
		if(File.Exists(userPath + "/savedUserDoesList.gd")) { 
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(userPath + "/savedUserDoesList.gd", FileMode.Open);
			userDoesList = (List<User.UserDoes>)bf.Deserialize(file);
			file.Close();

			return userDoesList;
		} else 
			return null;
	}

	public void SavePet (Pet pet) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(userPath + "/savedPet.gd");
		bf.Serialize(file, pet);
		file.Close();
	}

	public Pet LoadPet () {
		if(File.Exists(userPath + "/savedPet.gd")) { 
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(userPath + "/savedPet.gd", FileMode.Open);
			Pet pet = (Pet)bf.Deserialize(file);
			file.Close();

			return pet;
		} else 
			return null;
	}
}
