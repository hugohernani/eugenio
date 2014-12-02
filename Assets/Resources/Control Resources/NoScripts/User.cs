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

	// database hidden relational table
	List<Dictionary<string,string>> availableFoods;

	List<Game> games;

	List<Category> categories;
	List<Category> subCategories;
	List<Task> tasks;

	List<UserDoes> userDoesList;
	List<UserDoes> cachedUserDoesList;
	UserDoes tempUserDoes;

	int taskPoints; // acertos por fase
	int starsStage; // estrelas por fase

	int stage = 1;
	int subStage = 1;

	Task currentTask;
	Category currentCategory;
	Category currentSubCategory;

	[SerializeField]
	Pet currentPetStatus;

	private static volatile User instance;
	private static object syncRoot = new System.Object();
	
	User() {
		currentPetStatus = new Pet ();
		tasks = new List<Task> ();
		userDoesList = new List<UserDoes> ();
		cachedUserDoesList = new List<UserDoes> ();
		tempUserDoes = new UserDoes ();
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

	public Task getTask(string name, int mainCategoryId){
		foreach(Task task in tasks){
			if(task.CategoryId == mainCategoryId && task.Name == name){
				return task;
			}
		}
		return null;
	}

	public Task getTask(string name, int mainCategoryId, int subCategoryId){
		foreach(Task task in tasks){
			if(task.Name == name && task.CategoryId == mainCategoryId && task.SubCategoryId == subCategoryId){
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
		// TODO It is possible to save this current task in the database from here before jumping to the next
		DBTimeControlTask.END_TASK ();

		int indexCurrentTask = tasks.LastIndexOf (currentTask);
		currentTask = tasks.ElementAt (indexCurrentTask+1);
		currentTask.Available = true; // make the task available
		tasks [indexCurrentTask + 1] = currentTask;
		Debug.Log ("Task released: " + currentTask.ToString ()); 
	}

	public void releaseNextCategory ()
	{
		int lastCategoryPosition = categories.LastIndexOf(currentCategory);
		currentCategory = categories.ElementAt(lastCategoryPosition+1);
		currentCategory.Available = true; // make the category available
		categories [lastCategoryPosition + 1] = currentCategory;
		Debug.Log("Category released: " + currentCategory.ToString());
	}

	public void releaseNextSubCategory()
	{
		int lastSubCategoryPosition = subCategories.LastIndexOf (currentSubCategory);
		currentSubCategory = subCategories.ElementAt (lastSubCategoryPosition+1);
		currentSubCategory.Available = true; // make the subCategory available
		categories [lastSubCategoryPosition + 1] = currentSubCategory;
		Debug.Log ("SubCategory released: " + currentSubCategory.ToString ());
	}

	void setUserDoesFromCurrentList (int idTask)
	{
		foreach (UserDoes ud in userDoesList) {
			if(ud.TaskId == idTask && ud.UserId == this.id){
				this.starsStage = ud.Stars;
				tempUserDoes = ud;
			}
		}
	}

	public void SaveTaskName(string name){
		tempUserDoes.UserId = this.id;
		int idTask = 0;
		foreach (Task task in tasks) {
			if(task.Name == name)
				idTask = task.Id;
		}
		tempUserDoes.TaskId = idTask;
		if(userDoesList.Count != 0){
			setUserDoesFromCurrentList(idTask);
		}

	}

	public void SaveTaskHits(){
		tempUserDoes.Hits = this.taskPoints;
	}

	public void SaveTaskDateAdnDuration(float duration, DateTime date_open){
		tempUserDoes.Duration = duration;
		tempUserDoes.Date_user_did = date_open;
		tempUserDoes.Stars = this.starsStage;
		
		UserDoes userDoes = tempUserDoes; // separate in memory.
		int savedPosition = -1;
		for(int i = 0; i < userDoesList.Count; i++){
			if (userDoes.Id == userDoesList[i].Id){
				savedPosition = i;
			}
		}

		if(savedPosition != -1){ // replace
			userDoesList[savedPosition] = userDoes;
		}else{ // add
			userDoesList.Add(userDoes);
			cachedUserDoesList.Add(userDoes);
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

	public List<UserDoes> CachedUserDoesList {
		get {
			return this.cachedUserDoesList;
		}
		set {
			cachedUserDoesList = value;
		}
	}

	public bool RemoveUserTaskFromCachedList(UserDoes taskRelation){
		return this.cachedUserDoesList.Remove (taskRelation);
	}

	public void AddTaskByVerification(Task newTask){
		Debug.Log ("To add: " + newTask.ToString ());
		if(!tasks.Contains(newTask)){
			Debug.Log("Task added: " + newTask.ToString());
			tasks.Add(newTask);
		}
	}
	
	public void AddUserDoesByVerification(UserDoes newUd){
		if(!userDoesList.Contains(newUd)){
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

	public class UserDoes{
		int id;
		int userId;
		int taskId;
		int hits;
		int stars;
		float duration;
		DateTime date_user_did;
		int tentativa;

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

		public int Tentativa {
			get {
				return this.tentativa;
			}
			set {
				tentativa = value;
			}
		}

		public int Stars {
			get {
				return this.stars;
			}
			set {
				stars = value;
			}
		}



		public override string ToString ()
		{
			return string.Format ("[UserDoes: UserId={0}, TaskId={1}, Hits={2}, Stars={3}, Date_user_did={4}, Duration={5}, Tentativa={6}]", UserId, TaskId, Hits, Stars, Date_user_did.ToShortDateString(), Duration.ToString(), Tentativa);
		}

	}

}
