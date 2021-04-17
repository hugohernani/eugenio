using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class User {

	// database fields
	int id;
	int level_pet;
	float logged_time;
	string name;
	string password;
	int stars_qty; // moeda
	string school;
	string teacher;
	DateTime created;
	DateTime updated;

	// database hidden relational table
	List<Dictionary<string,string>> availableFoods;

	List<Game> games;

	List<Category> categories;
	List<Category> subCategories;
	List<Task> tasks;

	List<UserDoes> userDoesList;
	Queue<UserDoes> cachedUserDoesQueue;
//	UserDoes tempUserDoes;

	List<UserPlays> userPlaysList;
	Queue<UserPlays> cachedUserPlaysQueue;
	UserPlays tempUserPlays;

	int taskPoints; // acertos por fase
	int starsStage; // estrelas por fase
	bool hasPlayed = false;

	int stage = 1;
	int subStage = 1;

	int tempTaskId;
	Task currentTask;
	Category currentCategory;
	Category currentSubCategory;

	Game currentGame;

	[SerializeField]
	Pet currentPetStatus;

	private static volatile User instance;
	private static object syncRoot = new System.Object();
	
	User() {
		updated = DateTime.Now;
		currentPetStatus = new Pet ();
		tasks = new List<Task> ();
		games = new List<Game> ();
		userDoesList = new List<UserDoes> ();
		cachedUserDoesQueue = new Queue<UserDoes>();
		userPlaysList = new List<UserPlays> ();
		cachedUserPlaysQueue = new Queue<UserPlays> ();
//		tempUserDoes = new UserDoes ();
		tempUserPlays = new UserPlays ();
	}
	
	public static User getInstance{
		get {
			if (instance == null) {
				lock (syncRoot) {
					if (instance == null) 
						instance = new User();
				}
			}
			return instance;
		}
	}

	public List<Dictionary<string, string>> AvailableFoods{
		get {
			return this.availableFoods;
		}
		set {
			this.availableFoods = value;
		}
	}

	public List<Category> Categories {
		get {
			return this.categories;
		}
		set {
			categories = value;
		}
	}

	public List<Category> SubCategories {
		get {
			return this.subCategories;
		}
		set {
			subCategories = value;
		}
	}

	public Game getGame(string name){
		foreach (Game game in games) {
			if(game.Name == name){
				return game;
			}
		}
		return null;
	}

	public Task getTask(string name, int mainCategoryId){
		foreach(Task task in tasks){
			if(task.CategoryId == mainCategoryId && task.Name == name){
				return task;
			}
		}
		return null;
	}

	public Task getTask(int mainCategoryId, int subCategoryId){
		foreach(Task task in tasks){
			if(task.CategoryId == mainCategoryId && task.SubCategoryId == subCategoryId){
				return task;
			}
		}
		return null;
	}

	public Task getTask(string name, int mainCategoryId, int subCategoryId){
		foreach(Task task in tasks){
			if(task.Name == name && task.CategoryId == mainCategoryId && task.SubCategoryId == subCategoryId){
				Debug.Log(task.ToString());
				return task;
			}
		}
		return null;
	}

	public MainCategory getCategory(int mainCategoryId){
		foreach (Category item in categories) {
			if(item.Id == mainCategoryId){
				return (MainCategory) item;
			}
		}
		return null;
	}

	public Category getSubCategory(int subCategoryId){
		foreach (Category item in categories) {
			if(item.SubCategories != null){
				foreach (Category subCategory in item.SubCategories) {
					if(subCategory.Id == subCategoryId)
						return subCategory;
				}
			}
		}
		return null;
	}

	public List<Task> TasksByCategory (int categoryId){
		List<Task> stageTasks = new List<Task>();
		foreach (Task task in tasks) {
			if(task.CategoryId == categoryId){
				stageTasks.Add(task);
			}
		}
		return stageTasks;
	}

	public void releaseNextTask(){
		int indexCurrentTask = tasks.LastIndexOf (currentTask);
		currentTask = tasks.ElementAt (indexCurrentTask+1);
		currentTask.Available = true; // make the task available
		Debug.Log ("Task released: " + currentTask.ToString ()); 
	}

	public void releaseNextCategory ()
	{
		Debug.Log ("Last task before advancing to the next Category: " + this.currentTask.Name);
		this.CurrentStage++;
		int lastCategoryPosition = categories.LastIndexOf(currentCategory);
		currentCategory = categories.ElementAt(lastCategoryPosition+1);
		currentCategory.Available = true; // make the category available
		Debug.Log("Category released: " + currentCategory.ToString());
		releaseNextTask ();
	}

	public void releaseNextSubCategory()
	{
		int lastSubCategoryPosition = subCategories.LastIndexOf (currentSubCategory);
		currentSubCategory = subCategories.ElementAt (lastSubCategoryPosition+1);
		currentSubCategory.Available = true; // make the subCategory available
		categories [lastSubCategoryPosition + 1] = currentSubCategory;
		Debug.Log ("SubCategory released: " + currentSubCategory.ToString ());
		releaseNextTask ();
	}

	bool setUserPlaysFromCurrentList()
	{
		foreach (UserPlays up in userPlaysList) {
			if(up.UserId == this.Id && up.GameId == currentGame.Id){
				Debug.Log("Existed userPlays: " + up.ToString());
				tempUserPlays = up;
				return true;
			}
		}
		return false;
	}

//	bool setUserDoesFromCurrentList ()
//	{
//		foreach (UserDoes ud in userDoesList) {
//			if(ud.TaskId == currentTask.Id && ud.UserId == this.id){
//				this.stars_qty = ud.Stars;
//				tempUserDoes = ud;
//				Debug.Log("Existed userDoes: " + tempUserDoes.ToString());
//				return true;
//			}
//		}
//		this.stars_qty = 0;
//		return false;
//	}

	public void startSavingGame(){
		tempUserPlays.UserId = this.id;
		tempUserPlays.GameId = currentGame.Id;
		if(userPlaysList.Count != 0){
			setUserPlaysFromCurrentList();
		}
	}

	public void saveScore(){
		if(tempUserPlays.Record < tempUserPlays.Score){
			tempUserPlays.Record = tempUserPlays.Score;
		}

		tempUserPlays.Score = currentGame.CurrentScore;
		
		UserPlays userPlays = tempUserPlays; // separate in memory.
		int savedPosition = -1;
		for(int i = 0; i < userPlaysList.Count; i++){
			UserPlays tempUp = userPlaysList[i];
			if (userPlays.GameId == tempUp.GameId && userPlays.UserId == tempUp.UserId
			    && userPlays.Score < tempUp.Score){
				savedPosition = i;
			}
		}
		
		if(savedPosition != -1){ // replace
			Debug.Log("Replacing existed UserPlays");
			userPlaysList[savedPosition] = userPlays;
		}else{ // add
			Debug.Log("Adding UserPlays");
			userPlaysList.Add(userPlays);
			cachedUserPlaysQueue.Enqueue(userPlays);
		}

		this.tempUserPlays = new UserPlays ();

	}

	public bool StartSavingTask(){
		// setting starStage
		this.tempTaskId = currentTask.Id;
		int tempHit = 0;
		foreach (UserDoes ud in userDoesList) {
			if(ud.TaskId == currentTask.Id && ud.UserId == this.id){
				if(tempHit < ud.Hits){
					tempHit = ud.Hits;
				}
			}
		}
		this.starsStage = tempHit;
		if(tempHit != 0) return true;
		return false;
	}

//	public void SaveTaskHits(){
//		tempUserDoes.Hits = this.taskPoints;
//	}

	public void SaveTaskDateAndDuration(float duration){
		UserDoes userDoes = new UserDoes ();

		userDoes.UserId = this.Id;
		userDoes.TaskId = this.tempTaskId;
		userDoes.Hits = this.taskPoints;
		userDoes.Duration = duration;
		userDoes.Date_user_did = DateTime.Now;

		Debug.Log("Adding UserTask: " + userDoes.ToString());
		userDoesList.Add(userDoes);
		cachedUserDoesQueue.Enqueue(userDoes);
//		}

		this.starsStage = 0;
		this.taskPoints = 0;
//		this.hasPlayed = false;

	}

	public List<UserPlays> UserPlaysList {
		get {
			return this.userPlaysList;
		}
		set {
			userPlaysList = value;
		}
	}

	public Queue<UserPlays> CachedUserPlaysQueue {
		get {
			return this.cachedUserPlaysQueue;
		}
		set {
			cachedUserPlaysQueue = value;
		}
	}

	public List<UserDoes> UserDoesList {
		get {
			return this.userDoesList;
		}
		set {
			userDoesList = value;
		}
	}

	public Queue<UserDoes> CachedUserDoesQueue {
		get {
			return this.cachedUserDoesQueue;
		}
		set {
			cachedUserDoesQueue = value;
		}
	}

	public float CalculateExperience ()
	{
		float result = 0f;
		if(userDoesList.Count > tasks.Count){
			// TODO Deal when user has played some tasks a lot of times and userDoesList.Count pass through the quantity
			// of tasks.
			result = (tasks.Count % userDoesList.Count);
		}else{
			result = (userDoesList.Count % tasks.Count);
		}
		result /= 100f;
		currentPetStatus.Experience = result;
		return result;
	}


	public void UpdateEntertainment(){
		int referencial_value = 20;
		List<int> packages_number_games = new List<int>();
		foreach (Game game in games) {
			packages_number_games.Add(game.Id);
		}

		int indice_from_1 = packages_number_games.IndexOf (this.currentGame.Id) + 1; // to deal with package_number 0, index 0
		int level = this.level_pet + 1; // to deal with level 0.
		float porcentage = indice_from_1 / level;
		float value_increase = referencial_value * porcentage/100f;
		float petEntResult = this.currentPetStatus.Entertainment + value_increase;

		if (petEntResult > 1f)
			this.currentPetStatus.Entertainment = 1f;
		else
			this.currentPetStatus.Entertainment = petEntResult;
	}

	public void AddGameByVerification(Game game){
		if(!games.Contains(game)){
			games.Add(game);
		}
	}

	public void AddUserPlayByVerification(UserPlays up){
		if(!userPlaysList.Contains(up)){
			userPlaysList.Add(up);
		}
	}

	public void AddTaskByVerification(Task newTask){
		if(!tasks.Contains(newTask)){
			tasks.Add(newTask);
		}
	}
	
	public void AddUserDoesByVerification(UserDoes newUd){
		if (!userDoesList.Contains(newUd)) {
			userDoesList.Add(newUd);
		}
	}

	public Pet CurrentPetStatus {
		get {
			return this.currentPetStatus;
		}
		set {
			currentPetStatus = value;
		}
	}

	public int Level_pet {
		get {
			return this.level_pet;
		}
		set {
			level_pet = value;
		}
	}

	public float Logged_time {
		get {
			return this.logged_time;
		}
		set {
			logged_time = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public string Password {
		get {
			return this.password;
		}
		set {
			password = value;
		}
	}

	public int Stars_qty {
		get {
			return this.stars_qty;
		}
		set {
			stars_qty = value;
		}
	}

	public string School {
		get {
			return this.school;
		}
		set {
			school = value;
		}
	}

	public string Teacher {
		get {
			return this.teacher;
		}
		set {
			teacher = value;
		}
	}
	
	public int TaskPoints {
		get {
			return this.taskPoints;
		}
		set {
			taskPoints = value;
		}
	}

	public int Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public int StarsStage {
		get {
			return this.starsStage;
		}
		set {
			starsStage = value;
		}
	}

	public int CurrentStage {
		get {
			return this.stage;
		}
		set {
			stage = value;
		}
	}

	public int CurrentSubStage {
		get {
			return this.subStage;
		}
		set {
			subStage = value;
		}
	}

	public List<Game> Games {
		get {
			return this.games;
		}
		set {
			games = value;
		}
	}

	public int TotalTasks {
		get {
			return this.tasks.Count;
		}
	}

	public Game CurrentGame {
		get {
			return this.currentGame;
		}
		set {
			currentGame = value;
		}
	}
	
	public Task CurrentTask{
		set{
			currentTask = value;
		}
		get{
			return this.currentTask;
		}
	}

	public Category CurrentCategory {
		get {
			return this.currentCategory;
		}
		set {
			currentCategory = value;
		}
	}

	public Category CurrentSubCategory {
		get {
			return this.currentSubCategory;
		}
		set {
			currentSubCategory = value;
		}
	}

	public DateTime Updated {
		get {
			return this.updated;
		}
		set {
			updated = value;
		}
	}

	public bool HasPlayed {
		get {
			bool returned = !this.hasPlayed;
			this.hasPlayed = true;
			return returned;
		}
	}

	[Serializable]
	public class UserDoes: ICloneable{
		int id;
		int userId;
		int taskId;
		int hits;
		float duration;
		DateTime date_user_did;

		public UserDoes ()
		{
		}

		public int Id {
			get {
				return this.id;
			}
			set {
				id = value;
			}
		}

		public int UserId {
			get {
				return this.userId;
			}
			set {
				userId = value;
			}
		}

		public int TaskId {
			get {
				return this.taskId;
			}
			set {
				taskId = value;
			}
		}

		public int Hits {
			get {
				return this.hits;
			}
			set {
				hits = value;
			}
		}

		public float Duration {
			get {
				return this.duration;
			}
			set {
				duration = value;
			}
		}

		public DateTime Date_user_did {
			get {
				return this.date_user_did;
			}
			set {
				date_user_did = value;
			}
		}

		public object Clone ()
		{
			return this.MemberwiseClone ();
		}

		public override string ToString ()
		{
			return string.Format ("[UserDoes: UserId={0}, TaskId={1}, Hits={2}, Date_user_did={3}, Duration={4}]", UserId, TaskId, Hits, Date_user_did.ToShortDateString(), Duration.ToString());
		}

	}

	[Serializable]
	public class UserPlays: ICloneable{
		int id;
		int userId;
		int gameId;
		int score;
		int record;
		DateTime date_user_played;

		public int Id {
			get {
				return this.id;
			}
			set {
				id = value;
			}
		}

		public int UserId {
			get {
				return this.userId;
			}
			set {
				userId = value;
			}
		}

		public int GameId {
			get {
				return this.gameId;
			}
			set {
				gameId = value;
			}
		}

		public int Score {
			get {
				return this.score;
			}
			set {
				score = value;
			}
		}

		public int Record {
			get {
				return this.record;
			}
			set {
				record = value;
			}
		}

		public DateTime Date_user_played {
			get {
				return this.date_user_played;
			}
			set {
				date_user_played = value;
			}
		}

		public object Clone ()
		{
			return this.MemberwiseClone ();
		}

		public override string ToString ()
		{
			return string.Format ("[UserPlays: Id={0}, UserId={1}, GameId={2}, Score={3}, Record={4}, Date_user_played={5}]", Id, UserId, GameId, Score, Record, Date_user_played);
		}

	}

}
