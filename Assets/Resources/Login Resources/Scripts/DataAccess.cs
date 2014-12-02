using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic; 

public class DataAccess: MonoBehaviour{

	readonly string NAME_FIELD = "nome";
	readonly string PASSWORD_FIELD = "senha";

	private readonly string serverUriPath = "http://localhost:8000/";
	private readonly string userAccessUriPath = "User/";
	private readonly string foodAccessUriPath = "Food/";
	private readonly string TaskManagerAccessUriPath = "TaskManager/";
	private readonly string GameAccessUriPath = "Game/";
	private readonly string PetAccessUriPath = "VirtualPet/";

	private User user;
	private WWW wwwResquest;

	public static bool FINISH = false;

	bool nextTaskAvailable = true;

	private static int previousTaskId = -1;

	void Awake(){
		user = User.getInstance;
	}

	public void POST(string url, Dictionary<string,string> post)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		
		WaitForRequest(www);
	}

	public bool POSTConfirmation(string url, Dictionary<string, string> post)
	{
		WWWForm form = new WWWForm ();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		return WaitForRequestWithConfirmation (www);
	}

	public void GET(string url){
		wwwResquest = new WWW (url);

		WaitForRequest (wwwResquest);
	}


	public void GET(string url, string getArgument){
		wwwResquest = new WWW (url + "/" + getArgument);

		WaitForRequest (wwwResquest);
	}

	private IEnumerator ILoginUser(string username, string password, Action<string> info){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += "getUser/";
		
		Dictionary<string, string> userDict = new Dictionary<string, string> ();

		userDict.Add (NAME_FIELD, username);
		userDict.Add (PASSWORD_FIELD, password);

		POST (targetUri, userDict);

		string response = wwwResquest.text;

		if (response != "") {
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
			info("Usuario Encontrado. \n Level: " + user.Level_pet.ToString());
			yield return new WaitForSeconds(1f);
			setPetStatus ();
			info("Pet carregado.");
			yield return new WaitForSeconds(1f);
			setAvailableFood (user.Level_pet);
			info("Comidas disponiveis carregadas.");
			yield return new WaitForSeconds(1f);
			getAvailableGames ();
			info("Jogos disponiveis carregados.");
			yield return new WaitForSeconds(1f);
			retrieveCategories ();
			info("Categorias de tarefas carregadas.");
			yield return new WaitForSeconds(1f);
			info("Tarefas carregadas.");

			yield break;

		} else {

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
				action("Conexao ainda nao estabelecida.");
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
			yield return new WaitForSeconds (1f);
			yield return StartCoroutine(ILoginUser(username, password, action));
			yield return new WaitForSeconds(1f);
			action("Informaçoes adquiridas");
			yield return new WaitForSeconds(1f);
			action ("finished");
		}

	}

	private void setAvailableFood(int level){
		string targetUri = serverUriPath + foodAccessUriPath;
		targetUri += ("filter/" + level + "/");

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

	private void setPetStatus(){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("get/" + user.Id.ToString() + "/");

		GET (targetUri);
		string response = wwwResquest.text;

		if(response != ""){
			string[] statusObjects = response.Split(',');

			Debug.Log("Pet status: " + response);

			Pet petStatus = new Pet(
				float.Parse(statusObjects[0]),
				float.Parse(statusObjects[1]),
				float.Parse(statusObjects[2])
				);
			petStatus.Experience = float.Parse(statusObjects[3]);

			user.CurrentPetStatus = petStatus;

		}
	}

	public void updatePetStatus(float feed, float health, float entertainment, float experience){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("updatePet/" + user.Id.ToString() + "/" + feed.ToString() + "/" + health.ToString() +
		              entertainment.ToString() + "/" + experience.ToString() + "/");

		GET (targetUri);
		string response = wwwResquest.text;

		if (response == "1") {
			Debug.Log ("Pet updated in the server side");
		} else {
			Debug.Log("A problem occured updating the server side");

			// TODO Saving in file
		}
	}

	public void updatePetStatus(Pet pet){
		string targetUri = serverUriPath + PetAccessUriPath;
		targetUri += ("updatePet/" + user.Id.ToString() + "/" + pet.Feed.ToString() + "/" + pet.Health.ToString() +
		              pet.Entertainment.ToString() + "/" + pet.Experience.ToString() + "/");
		
		GET (targetUri);
		string response = wwwResquest.text;
		
		if (response == "1") {
			Debug.Log ("Pet updated in the server side");
		} else {
			Debug.Log("A problem occured updating the pet in the server side");

			// TODO Saving in file.
		}
	}


	private void getAvailableGames(){
		string targetUri = serverUriPath + GameAccessUriPath;
		targetUri += "all/";

		GET (targetUri);
		string response = wwwResquest.text;

		List<Game> games = new List<Game> ();

		if(response != ""){

			string[] gameInfos = response.Split(';');
			foreach (string gameInfo in gameInfos) {
				string[] values = gameInfo.Split('.');
				Game game = new Game();
				game.Id = int.Parse(values[0]);
				game.Name = values[1];
				games.Add(game);
			}
		}

		user.Games = games;
	}

	private void retrieveCategories(){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("categories/all/" + user.Level_pet + "/" + user.CurrentStage + "/");

		GET (targetUri);
		string response = wwwResquest.text;

		List<Category> categories = new List<Category> ();

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
			if(category.Available){
				nextTaskAvailable = true;
				category.SubCategories = retrieveSubCategories(category, category.Stage);
			}

			categories.Add(category);
		}

		user.Categories = categories;
	}

	private List<Category> retrieveSubCategories(Category category, int categoryStage){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("subCategories/filter/" + category.Id + "/" + user.CurrentStage + "/" 
		              + categoryStage + "/" + user.CurrentSubStage + "/");

		GET (targetUri);
		string response = wwwResquest.text;

		if(response == ""){
			retrieveTasksAndUserDoes(category, null);
			return null; // There is not subcategory
		}else{
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

			Debug.Log(subCategories.Count + " Subcategorias da categoria " + ((MainCategory) category).Name + ", " + category.Id + " adicionadas.");

			return subCategories;
		}
	}

	private void retrieveTasksAndUserDoes(Category category, Category subCategory){
		string targetUri = serverUriPath + TaskManagerAccessUriPath;
		targetUri += ("tasks/filter/" + user.Id + "/" + category.Id + "/");

		if (subCategory != null){
			targetUri += (subCategory.Id + "/");
		}

		GET (targetUri);
		string response = wwwResquest.text;

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

			user.AddTaskByVerification(task);

			if(values[5] != "0"){
				task.Available = (nextTaskAvailable = true);

				string[] udValues = values[5].Split('|');
				Debug.Log(values[5]);
				User.UserDoes ud = new User.UserDoes();
				ud.Id = int.Parse(udValues[0]);
				ud.UserId = int.Parse(udValues[1]);
				ud.TaskId = int.Parse(udValues[2]);
				ud.Hits = int.Parse(udValues[3]);
				ud.Stars = int.Parse(udValues[4]);
				ud.Duration = float.Parse(udValues[5]);
				ud.Date_user_did = DateTime.ParseExact(udValues[6], "yyyy-MM-dd HH:mm:ss", null);
				ud.Tentativa = int.Parse(udValues[7]);


				user.AddUserDoesByVerification(ud);
			}else{
				Debug.Log("Guaranteed: " + nextTaskAvailable);
				task.Available = nextTaskAvailable;
				nextTaskAvailable = false;

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

		List<User.UserDoes> tempListUserDoes = new List<User.UserDoes> ();

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
			ud.Tentativa = int.Parse(values[4]);
			ud.TaskId = int.Parse(values[5]);
			ud.UserId = int.Parse(values[6]);

			tempListUserDoes.Add(ud);

			Debug.Log("Added a relational userTask");
		}

		user.UserDoesList = tempListUserDoes;
		user.CachedUserDoesList = tempListUserDoes;

	}

	public void createUpdateUserDoes(){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("createUpdateUserDoes/");

		List<User.UserDoes> itemsToRemoveFromList = new List<User.UserDoes> ();

		foreach (User.UserDoes userDoes in user.CachedUserDoesList) {
			Dictionary<string, string> userDoesDict = new Dictionary<string, string> ();

			userDoesDict.Add("user", userDoes.UserId.ToString());
			userDoesDict.Add("task", userDoes.TaskId.ToString());
			userDoesDict.Add("hits", userDoes.Hits.ToString());
			userDoesDict.Add("stars", userDoes.Stars.ToString());
			userDoesDict.Add("duration",userDoes.Duration.ToString());
			userDoesDict.Add("date_user_did", userDoes.Date_user_did.ToString("yyyy-MM-dd HH:mm:ss"));

			bool result = POSTConfirmation(targetUri, userDoesDict);

			if(result){
				Debug.Log("UserTask tuple created/updated in database");
			}else{
				// TODO save in file.

				Debug.Log("UserTask saved in file"); // TODO
			}
			itemsToRemoveFromList.Add(userDoes);
		}

		foreach (User.UserDoes ud in itemsToRemoveFromList) {
			user.RemoveUserTaskFromCachedList(ud);
		}

	}

	public void updateUserInfo(){
		string targetUri = serverUriPath + userAccessUriPath;
		targetUri += ("update/");

		Dictionary<string, string> userDict = new Dictionary<string, string> ();

		userDict.Add("name", user.Name);
		userDict.Add ("password", user.Password);
		userDict.Add("level", user.Level_pet.ToString ());
		userDict.Add("logged_time", user.Logged_time.ToString());
		userDict.Add("currentStage", user.CurrentStage.ToString());
		userDict.Add("currentSubStage",user.CurrentSubStage.ToString());
		userDict.Add("stars", user.Stars_qty.ToString());

		bool result = POSTConfirmation(targetUri, userDict);
		
		if(result){

			string response = wwwResquest.text;

			if (response == "1") {
				Debug.Log ("User updated in the server side");
			} else {
				Debug.Log("A problem occured updating the User in the server side");
				
				// TODO Saving in file due to problem in the server side
			}
		}else{
			// TODO save in file due to problem with conection.
			
			Debug.Log("User saved in file"); // TODO
		}

		updatePetStatus (user.CurrentPetStatus);

	}

	private void WaitForRequest(WWW www)
	{

		while(!www.isDone){
			CheckDatabase.UriReach = www.url;
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

	private bool WaitForRequestWithConfirmation(WWW www){

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
