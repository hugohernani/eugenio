﻿using UnityEngine;
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
		updated = DateTime.Now;
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

	public Task getTask(int mainCategoryId, int subCategoryId){
		foreach(Task task in tasks){
			if(task.CategoryId == mainCategoryId && task.SubCategoryId == subCategoryId){
				Debug.Log("T: " + task.ToString());
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

	void setUserDoesFromCurrentList ()
	{
		foreach (UserDoes ud in userDoesList) {
			if(ud.TaskId == currentTask.Id && ud.UserId == this.id){
				Debug.Log("Existed userDoes: " + ud.ToString());
				this.starsStage = ud.Stars;
				tempUserDoes = ud;
				return;
			}
		}
		this.starsStage = 0;
	}

	public void StartSavingTask(){
		tempUserDoes.UserId = this.id;
		tempUserDoes.TaskId = currentTask.Id;
		if(userDoesList.Count != 0){
			setUserDoesFromCurrentList();
		}

	}

	public void SaveTaskHits(){
		tempUserDoes.Hits = this.taskPoints;
	}

	public void SaveTaskDateAdnDuration(float duration){
		tempUserDoes.Duration = duration;
		tempUserDoes.Date_user_did = DateTime.Now;;
		tempUserDoes.Stars = this.starsStage;
		
		UserDoes userDoes = tempUserDoes; // separate in memory.
		int savedPosition = -1;
		for(int i = 0; i < userDoesList.Count; i++){
			if (userDoes.Id == userDoesList[i].Id){
				savedPosition = i;
			}
		}

		if(savedPosition != -1){ // replace
			Debug.Log("Replacing existed UserDoes");
			userDoesList[savedPosition] = userDoes;
		}else{ // add
			Debug.Log("Adding UserDoes");
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

	public float CalculateExperience ()
	{
		int qtyTasksPlayed = 0;
		foreach(Task task in tasks){
			if(task.Available){
				qtyTasksPlayed++;
			}
		}
		float result = (tasks.Count / (qtyTasksPlayed - 1)) / 100;
		currentPetStatus.Experience = result;
		Debug.Log(result);
		return result;
	}

//	public int CalculateMoney(){
//		int qtyMoney = 0;
//		foreach (User.UserDoes ud in userDoesList) {
//			qtyMoney += ud.Stars;
//		}
//		return qtyMoney;
//	}

	public bool RemoveUserTaskFromCachedList(UserDoes taskRelation){
		return this.cachedUserDoesList.Remove (taskRelation);
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

	[Serializable]
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
