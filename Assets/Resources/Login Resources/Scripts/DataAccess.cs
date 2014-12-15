using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic; 
using System.IO;

public class DataAccess: MonoBehaviour{
	
	readonly string NAME_FIELD = "nome";
	readonly string PASSWORD_FIELD = "senha";

//	readonly string serverUriPath = "http://localhost:8000/";
	readonly string serverUriPath = "http://hhernanni.webfactional.com/";
	readonly string userAccessUriPath = "user/";
	readonly string foodAccessUriPath = "food/";
	readonly string TaskManagerAccessUriPath = "taskmanager/";
	readonly string GameAccessUriPath = "game/";
	readonly string PetAccessUriPath = "virtualpet/";
	
	User user;
	WWW wwwResquest;

	// control variables
	bool nextTaskAvailable = true;	
	private static int previousTaskId = -1;

	SaveLoad saveLoad;
	
	void Awake(){
		user = User.getInstance;
		saveLoad = new SaveLoad (user.Name);
	}

	void POST(string url, Dictionary<string,string> post)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		
		WaitForRequest(www);
	}
	
	bool POSTConfirmation(string url, Dictionary<string, string> post)
	{
		WWWForm form = new WWWForm ();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		
		return WaitForRequestWithConfirmation (www);
	}
	
	void GET(string url){
		wwwResquest = new WWW (url);
		
		WaitForRequest (wwwResquest);
	}
	
	
	void GET(string url, string getArgument){
		wwwResquest = new WWW (url + "/" + getArgument);
		
		WaitForRequest (wwwResquest);
	}

	IEnumerator commitFiles(){
		string[] usersFoldersPath = Directory.GetDirectories(Application.persistentDataPath);
		string[] usersFoldersNames = new string[usersFoldersPath.Length];
		for(int i = 0; i < usersFoldersPath.Length; i++){
			string path = usersFoldersPath[i];
			int lastIndexSlash = path.LastIndexOf("\\");
			usersFoldersNames[i] = path.Substring(lastIndexSlash+1);
		}
		if(usersFoldersPath.Length != 0){
			for(int i = 0; i < usersFoldersPath.Length; i++){
				if(Directory.GetFiles(usersFoldersPath[i]).Length != 0){
					string userName = usersFoldersNames[i];
					SaveLoad saveLoadInstance = new SaveLoad(userName);
					updateInfoInDatabaseFromFile(saveLoadInstance);
					saveLoadInstance.destroyUserFolder(userName);
				}
			}
		}else{
			Debug.Log("There are not users informations to be commited to database");
		}
		yield break;
	}

	
	IEnumerator ILoginUser(string username, string password, Action<string> info){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += "getUser/";
		
		Dictionary<string, string> userDict = new Dictionary<string, string> ();
		
		userDict.Add (NAME_FIELD, username);
		userDict.Add (PASSWORD_FIELD, password);
		
		POST (targetUri, userDict);
		
		string response = wwwResquest.text;

		if (response.Substring(0,2) != "-1") {
			string[] textsplited = response.Split (',');
			
			user.Id = int.Parse (textsplited [0]);
			user.Name = textsplited [1];
			user.Password = textsplited [2];
			user.Level_pet = int.Parse (textsplited [3]);
			user.Logged_time = float.Parse (textsplited [4]);
			user.CurrentStage = int.Parse (textsplited [5]);
			user.CurrentSubStage = int.Parse (textsplited [6]);
			user.Stars_qty = int.Parse (textsplited [7]);
			user.School = textsplited [8];
			user.Teacher = textsplited [9];
			user.Updated = DateTime.ParseExact(textsplited[10], "yyyy-MM-dd HH:mm:ss", null);
			info("Usuario Encontrado. \n Level: " + user.Level_pet.ToString());
			yield return new WaitForSeconds(1f);
			setPetStatus ();
			info("Pet carregado.");
			yield return new WaitForSeconds(1f);
			getAvailableFoods ();
			info("Comidas disponiveis carregadas.");
			yield return new WaitForSeconds(1f);
			getAvailableGames ();
			info("Jogos disponiveis carregados.");
			yield return new WaitForSeconds(1f);
			retrieveCategories ();
			info("Categorias de tarefas carregadas.");
			yield return new WaitForSeconds(1f);
			info("Tarefas carregadas.");

			yield return new WaitForSeconds(1f);
			info("Informacoes adquiridas");
			yield return new WaitForSeconds(1f);
			info("finished");

			yield break;

		} else {

			info(response.Substring(2));
			yield break;
		}
		
		
		
	}

	// TODO Until now just verify the connection with the server.
	IEnumerator testConnection (Action<string> action, Action<bool> result)
	{
		bool availability;
		
		switch(Application.internetReachability){
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			availability = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			availability = true; // this can be changed to deal with redirection...
			break;
		case NetworkReachability.NotReachable:
			availability = false;
			break;
		default:
			availability = false;
			break;
			
		}
		if(availability){
			WWW wwwTest = new WWW(serverUriPath); // server url
			while(!wwwTest.isDone){ // It would be better using a coroutine. Maybe later.
				action("Tentando estabelecer conexao. \n Por favor, aguarde.");
			}
			if(wwwTest.error == null){
				result(true);
			}else{
				action("Houve algum problema na conexao com o servidor.");
				result(false);
			}
		}else{
			action("Sem conexao.");
		}
		
		yield return null;
		
	}
	
	public IEnumerator LoginUser(string username, string password, Action<string> action){
		
		action("Verificando conexao");
		bool result = false;
		yield return StartCoroutine (testConnection (action, (connected) =>{
			result = connected;
		}));
		if(result){
			this.saveLoad = new SaveLoad(username);

			yield return StartCoroutine(commitFiles ()); // This method will save in database each file saved before when there weren't connection.
//			yield return new WaitForSeconds (3f);
			yield return StartCoroutine(ILoginUser(username, password, action));
		}
		
	}

	public void getAvailableFoods(){
		string targetUri = serverUriPath + foodAccessUriPath;
		targetUri += ("filter/" + user.Level_pet + "/");
		
		GET(targetUri);
		string response = wwwResquest.text;
		
		if(response != ""){
			List<Dictionary<string, string>> listFoods = new List<Dictionary<string, string>>();
			
			string[] foodObjects = response.Split(';');
			foreach (string food in foodObjects) {
				string[] values = food.Split(',');
				Dictionary<string, string> fDic = new Dictionary<string, string>();
				fDic.Add("name", values[1]);
				fDic.Add("value", values[2]);
				listFoods.Add(fDic);
			}
			
			user.AvailableFoods = listFoods;
		}
	}
	
	void setPetStatus(){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("get/" + user.Id.ToString() + "/");
		
		GET (targetUri);
		string response = wwwResquest.text;
		
		if(response != ""){
			string[] statusObjects = response.Split(',');
			
			Pet petStatus = new Pet(
				float.Parse(statusObjects[0]),
				float.Parse(statusObjects[1]),
				float.Parse(statusObjects[2])
				);
			petStatus.UserId = user.Id;
			petStatus.Experience = float.Parse(statusObjects[3]);

			user.CurrentPetStatus = petStatus;
			
		}
	}
	
	void updatePetStatus(float feed, float health, float entertainment, float experience){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("updatePet/" + user.Id.ToString() + "/" + feed.ToString() + "/" + health.ToString() +
		              entertainment.ToString() + "/" + experience.ToString() + "/");
		
		GET (targetUri);
		string response = wwwResquest.text;
		
		if (response == "1") {
			Debug.Log ("Pet updated in the server side");
		} else {
			saveLoad.SavePet(user.CurrentPetStatus);
			Debug.Log("A problem occured updating the server side. Saving Pet in file");	 
		}
	}
	
	bool updatePetStatus(Pet pet){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("updatePet/" + pet.UserId.ToString() + "/" + pet.Feed.ToString() + "/" + pet.Health.ToString() +
		              "/" + pet.Entertainment.ToString() + "/" + pet.Experience.ToString() + "/");

		GET (targetUri);
		string response = wwwResquest.text;
		
		if (response == "1") {
			Debug.Log ("Pet updated in the server side");
			return true;
		} else {
			saveLoad.SavePet(pet);
			Debug.Log("A problem occured updating the server side. Saving Pet in file");
			return false;
		}
	}

	public void getAvailableGames(){
		string targetUri = serverUriPath + GameAccessUriPath;
		targetUri += ("all/" + user.Id.ToString() + "/");

		GET (targetUri);
		string response = wwwResquest.text;
		
		List<Game> games = new List<Game> ();
		
		if(response != ""){
			
			string[] gameInfos = response.Split(';');
			foreach (string gameInfo in gameInfos) {
				string[] values = gameInfo.Split('*');
				Game game = new Game();
				game.Id = int.Parse(values[0]);
				game.Name = values[1];
				game.Available = bool.Parse(values[2]);

				user.AddGameByVerification(game);

				if(values[3] != "0"){ // User already played this game
					string[] upValues = values[3].Split('|');
					User.UserPlays up = new User.UserPlays();

					up.Id = int.Parse(upValues[0]);
					up.UserId = int.Parse(upValues[1]);
					up.GameId = int.Parse(upValues[2]);
					up.Score = int.Parse(upValues[5]);
					up.Date_user_played = DateTime.ParseExact(upValues[4], "yyyy-MM-dd HH:mm:ss", null);
					up.Record = int.Parse(upValues[5]);

					user.AddUserPlayByVerification(up);

					game.CurrentScore = up.Score;
					game.CurrentRecord = up.Record;

				}
			}
		}
	}
	
	void retrieveCategories(){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("categories/all/" + user.Level_pet + "/" + user.CurrentStage + "/");
		
		GET (targetUri);
		string response = wwwResquest.text;
		
		List<Category> categories = new List<Category> ();

		if(response != ""){
			string[] categoryObjects = response.Split (';');
			foreach(string sCategory in categoryObjects){
				string[] values = sCategory.Split('.');
				MainCategory category = new MainCategory();
				category.Id = int.Parse(values[0]);
				category.Name = values[1];
				category.Stage = int.Parse(values[2]);
				category.InitialValue = int.Parse(values[3]);
				category.FinalValue = int.Parse(values[4]);
				category.Level = int.Parse(values[5]);
				category.Available = bool.Parse(values[6]);
				
				nextTaskAvailable = true;
				category.SubCategories = retrieveSubCategories(category, category.Stage);
				
				categories.Add(category);
			}
		}

		user.Categories = categories;
	}
	
	List<Category> retrieveSubCategories(Category category, int categoryStage){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("subCategories/filter/" + category.Id + "/" + user.CurrentStage + "/" 
		              + categoryStage + "/" + user.CurrentSubStage + "/");
		
		GET (targetUri);
		string response = wwwResquest.text;
		
		if(response == ""){
			retrieveTasksAndUserDoes(category, null);
			return null; // There is not subcategory
		}else{
			Debug.Log("category: " + ((MainCategory) category).ToString());
			List<Category> subCategories = new List<Category>();
			
			string[] subCategoryObjects = response.Split (';');
			foreach(string subCategoryS in subCategoryObjects){
				string[] values = subCategoryS.Split('.');
				MathSubCategory subCategory = new MathSubCategory();
				subCategory.Id = int.Parse(values[0]);
				subCategory.Category = category;
				subCategory.SubStage = int.Parse(values[2]);
				subCategory.Integer = bool.Parse(values[3]);
				int value = int.Parse(values[4]);
				if(value != 0)
					subCategory.Value = value;
				else
					subCategory.Random = true;
				subCategory.Available = bool.Parse(values[5]);
				subCategories.Add(subCategory);
				
				retrieveTasksAndUserDoes(category, subCategory);
				
			}
			
			return subCategories;
		}
	}
	
	void retrieveTasksAndUserDoes(Category category, Category subCategory){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("tasks/filter/" + user.Id + "/" + category.Id + "/");

		bool nextStageAvailable = false;

		if (subCategory != null){
			targetUri += (subCategory.Id + "/");
		}

		GET (targetUri);
		string response = wwwResquest.text;

		if(response != ""){
			string[] taskObjects = response.Split (';');
			foreach(string taskObject in taskObjects){
				string[] values = taskObject.Split('*');
				Task task = new Task();
				task.Id = int.Parse(values[0]);
				task.Name = values[1];
				task.CategoryId = int.Parse(values[3]);
				if (values[4] != "0")
					task.SubCategoryId = int.Parse(values[4]);
				else
					task.SubCategoryId = 0;
				
				if(values[5] != "0"){
					task.Available = true;
					
					string[] udValues = values[5].Split('|');
					User.UserDoes ud = new User.UserDoes();
					ud.Id = int.Parse(udValues[0]);
					ud.UserId = int.Parse(udValues[1]);
					ud.TaskId = task.Id;
					ud.Hits = int.Parse(udValues[3]);
					ud.Duration = float.Parse(udValues[4]);
					ud.Date_user_did = DateTime.ParseExact(udValues[5], "yyyy-MM-dd HH:mm:ss", null);
					
					if(ud.Hits < 10){
						nextTaskAvailable = false;
					}else{
						nextTaskAvailable = true;
					}
					
					user.AddUserDoesByVerification(ud);
				}else{
					task.Available = nextTaskAvailable;
					nextTaskAvailable = false;
				}
				
				user.AddTaskByVerification(task);
				
			}
		}

	}
	
	void retrieveUserDoes(int task_id){
		if (previousTaskId >= task_id)
			return;
		
		previousTaskId = task_id;
		
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("retrieveUserDoes/");
		
		Dictionary<string, string> postDict = new Dictionary<string, string> ();
		
		postDict.Add("user_id", user.Id.ToString());
		postDict.Add("task_id", task_id.ToString());
		
		POST (targetUri, postDict);
		Queue<User.UserDoes> tempQueueUserDoes = new Queue<User.UserDoes> ();

		string response = wwwResquest.text;
		
		if (response == "")
			return;
		
		string[] userDoesSList = response.Split(';');
		foreach (string userDoesS in userDoesSList) {
			string[] values = userDoesS.Split('.');
			
			User.UserDoes ud = new User.UserDoes();
			ud.Id = int.Parse(values[0]);
			ud.Hits = int.Parse(values[1]);
			ud.Duration = float.Parse(values[2]);
			ud.Date_user_did = DateTime.ParseExact(values[3], "yyyy-MM-dd HH:mm:ss", null);
			ud.TaskId = int.Parse(values[5]);
			ud.UserId = int.Parse(values[6]);

			tempQueueUserDoes.Enqueue(ud);

			Debug.Log("Added a relational userTask");
		}
		
		user.UserDoesList = tempQueueUserDoes.ToList();
//		user.CachedUserDoesQueue = tempQueueUserDoes;
		
	}

	bool createUpdateUserPlays (List<User.UserPlays> userPlaysLoadedList)
	{
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("saveScores/");
		
		int qntSavedInDB = 0;
		bool success = false;
		foreach (User.UserPlays up in userPlaysLoadedList) {
			Dictionary<string, string> userPlaysDict = new Dictionary<string, string> ();
			
			userPlaysDict.Add("user", up.UserId.ToString());
			userPlaysDict.Add("game", up.GameId.ToString());
			userPlaysDict.Add("score", up.Score.ToString());

			bool result = POSTConfirmation(targetUri, userPlaysDict);
			
			if(result){
				qntSavedInDB++;
				Debug.Log("UserPlays tuple created/updated in database");
			} else {
				saveLoad.AddUserPlays(up);
				Debug.Log("UserPlays saved in file");
			}
		}
		
		if(qntSavedInDB != 0 && qntSavedInDB == userPlaysLoadedList.Count){
			success = true;
		}
		
		if(saveLoad != null)
			saveLoad.SaveUserDoes ();
		
		return success;
	}

	bool createUpdateUserPlays ()
	{
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("saveScores/");

		int qntSavedInDB = 0;
		bool success = false;
		Queue<User.UserPlays> qTempUserPlays = user.CachedUserPlaysQueue;
		int qntQueued = qTempUserPlays.Count;
		while(qTempUserPlays.Count > 0){
			User.UserPlays up = qTempUserPlays.Dequeue();
			Dictionary<string, string> userPlaysDict = new Dictionary<string, string> ();
			
			userPlaysDict.Add("user", up.UserId.ToString());
			userPlaysDict.Add("game", up.GameId.ToString());
			userPlaysDict.Add("score", up.Score.ToString());
			
			bool result = POSTConfirmation(targetUri, userPlaysDict);
			
			if(result){
				qntSavedInDB++;
				Debug.Log("UserPlays tuple created/updated in database");
			} else {
				saveLoad.AddUserPlays(up);
				Debug.Log("UserPlays saved in file");
			}
		}
		
		if(qntSavedInDB != 0 && qntSavedInDB == qntQueued){
			success = true;
		}
		
		if(saveLoad != null){
			saveLoad.SaveUserPlays ();
		}

		user.CachedUserPlaysQueue = qTempUserPlays;
		return success;
	}

	// this is dealt a litle different from the overload method because doesn't need remove from userDoesList their items.
	bool createUpdateUserDoes(List<User.UserDoes> userDoesList){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("createUpdateUserDoes/");

		int qntSavedInDB = 0;
		bool success = false;
		foreach (User.UserDoes userDoes in userDoesList) {
			Dictionary<string, string> userDoesDict = new Dictionary<string, string> ();
			
			userDoesDict.Add("user", userDoes.UserId.ToString());
			userDoesDict.Add("task", userDoes.TaskId.ToString());
			userDoesDict.Add("hits", userDoes.Hits.ToString());
			userDoesDict.Add("duration",userDoes.Duration.ToString());
			userDoesDict.Add("date_user_did", userDoes.Date_user_did.ToString("yyyy-MM-dd HH:mm:ss"));
			
			bool result = POSTConfirmation(targetUri, userDoesDict);
			
			if(result){
				qntSavedInDB++;
				Debug.Log("UserTask tuple created/updated in database");
			} else {
				saveLoad.AddUserDoes(userDoes);
				Debug.Log("UserTask saved in file");
			}
		}

		if(qntSavedInDB != 0 && qntSavedInDB == userDoesList.Count){
			success = true;
		}

		if(saveLoad != null)
			saveLoad.SaveUserDoes ();

		return success;

	}
	
	bool createUpdateUserDoes(){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("createUpdateUserDoes/");
		
		int qntSavedInDB = 0;
		bool success = false;
		Queue<User.UserDoes> qTempUserDoes = user.CachedUserDoesQueue;
		int qntQueued = qTempUserDoes.Count;
		while(qTempUserDoes.Count > 0){
			User.UserDoes userDoes = qTempUserDoes.Dequeue();
			Dictionary<string, string> userDoesDict = new Dictionary<string, string> ();
			
			userDoesDict.Add("user", userDoes.UserId.ToString());
			userDoesDict.Add("task", userDoes.TaskId.ToString());
			userDoesDict.Add("hits", userDoes.Hits.ToString());
			userDoesDict.Add("duration",userDoes.Duration.ToString());
			userDoesDict.Add("date_user_did", userDoes.Date_user_did.ToString("yyyy-MM-dd HH:mm:ss"));
			
			bool result = POSTConfirmation(targetUri, userDoesDict);
			
			if(result){
				qntSavedInDB++;
				Debug.Log("UserTask tuple created/updated in database. " + userDoes.ToString());
			} else {
				saveLoad.AddUserDoes(userDoes);
				Debug.Log("UserTask saved in file");
			}

		}

		if(qntSavedInDB != 0 && qntSavedInDB == qntQueued){
			success = true;
		}

		user.CachedUserDoesQueue = qTempUserDoes;
		return success;
		
	}

	bool updateUserInfo(){
		Dictionary<string, string> userDict = new Dictionary<string, string> ();
		
		userDict.Add("name", user.Name);
		userDict.Add ("password", user.Password);
		userDict.Add("level", user.Level_pet.ToString ());
		userDict.Add("logged_time", user.Logged_time.ToString());
		userDict.Add("currentStage", user.CurrentStage.ToString());
		userDict.Add("currentSubStage",user.CurrentSubStage.ToString());
		userDict.Add("stars", user.Stars_qty.ToString());
		userDict.Add("updated", user.Updated.ToString("yyyy-MM-dd HH:mm:ss"));

		return updateUserInfo (userDict);

	}

	bool updateUserInfo(Dictionary<string, string> userDict){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("update/");

		bool result = POSTConfirmation(targetUri, userDict);
		
		if(result){
			
			string response = wwwResquest.text;
			
			if (response != "0" && response.Length >= 1) {
				Debug.Log ("User updated in the server side");
				Debug.Log(response);
				return true;
			} else {
				Debug.Log("A problem occured updating the User in the server side");
				saveLoad.SaveUserDict(userDict);
				return false;
			}
		}else{
			saveLoad.SaveUserDict(userDict);
			Debug.Log("User saved in file");
			return false;

		}

		return result;
	}

	void updateInfoInDatabaseFromFile(SaveLoad saveLoadInstance = null){
		if(saveLoadInstance != null){
			updateUserFromFile (saveLoadInstance);
			updateUserDoesFromFile (saveLoadInstance);
			updateUserPlaysFromFile(saveLoadInstance);
		}else{
			updateUserFromFile (saveLoad);
			updateUserDoesFromFile (saveLoad);
			updateUserPlaysFromFile(saveLoad);
			saveLoad.destroyUserFolder ();
		}
		// reconstruct saveLoad object?
	}

	// kind of template pattern
	void updateUserFromFile(SaveLoad saveLoadInstance){
		Dictionary<string, string> userDict = saveLoadInstance.LoadUserDict ();
		if (userDict != null) {
			updateUserInfo (userDict);
		}
		Pet petLoaded = saveLoadInstance.LoadPet ();
		if(petLoaded != null){
			updatePetStatus (petLoaded);
		}
	}

	void updateUserPlaysFromFile(SaveLoad saveLoadInstance){
		List<User.UserPlays> userPlaysLoadedList = saveLoadInstance.LoadUserPlaysList ();
		if(userPlaysLoadedList != null)
			createUpdateUserPlays(userPlaysLoadedList);
	}

	void updateUserDoesFromFile(SaveLoad saveLoadInstance){
		List<User.UserDoes> userDoesLoadedList = saveLoadInstance.LoadUserDoesList ();
		if(userDoesLoadedList != null)
			createUpdateUserDoes (userDoesLoadedList);
	}

	// kind of template pattern
	public void trySaveUserInformationsOnDB ()
	{
		if(user != null){
			this.saveLoad = new SaveLoad (user.Name);
			int qntSuccess = 0;
			if(updateUserInfo()){
				qntSuccess++;
			}
			
			if(updatePetStatus(user.CurrentPetStatus)){
				qntSuccess++;
			}
			
			if(createUpdateUserDoes()){
				qntSuccess++;
			}

			if(createUpdateUserPlays()){
				qntSuccess++;
			}
			
			if(qntSuccess != 4){
				updateInfoInDatabaseFromFile(saveLoad);
			}else{
				Debug.Log("All information saved");
			}
		}else{
			// TODO something else.
			Debug.Log("User is not logged.");
		}
	}
	
	void WaitForRequest(WWW www) {
		
		while(!www.isDone){
		}
		
		wwwResquest = www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			
			
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}
	
	bool WaitForRequestWithConfirmation(WWW www) {
		
		while(!www.isDone){
		}
		
		wwwResquest = www;
		
		bool result = false;
		
		// check for errors
		if (www.error == null)
		{
			result = true;
			
		} else{
			result = false;
		}
		return result;
	}
	
}
