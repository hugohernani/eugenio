using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*********************/
/* SAVE DATA IN FILE */
/*********************/

public class SaveLoad {

	public List<User.UserDoes> savedUserDoes = new List<User.UserDoes>();

	public List<Pet> savedPet = new List<Pet>();






	public static void SaveUserDoes () {

		SaveLoad.savedUserDoes.Add(User.UserDoes.current);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedUserDoes.gd");
		bf.Serialize(file, SaveLoad.savedUserDoes);
		file.Close();
	}

	public static void LoadUserDoes () {
		
		if(File.Exists(Application.persistentDataPath + "/savedUserDoes.gd")) { 
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedUserDoes.gd", FileMode.Open);
			SaveLoad.savedUserDoes = (List<User.UserDoes>)bf.Deserialize(file);
			file.Close();
		}
	}


	public void Save (Pet pet) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedPet.gd");
		bf.Serialize(file, pet);
		file.Close();
	}



	public Pet LoadPet () {
		
		if(File.Exists(Application.persistentDataPath + "/savedPet.gd")) { 
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedPet.gd", FileMode.Open);
			Pet pet = bf.Deserialize(file);
			file.Close();

			return pet
		}
	}
}
