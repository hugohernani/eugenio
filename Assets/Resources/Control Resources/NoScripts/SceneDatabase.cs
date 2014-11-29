using System;
using System.Collections.Generic;

public sealed class SceneDatabase{

	Stack<int> scenes;
	string mainScene;

	int categoryType = -1;

	SceneDatabase() {
		scenes = new Stack<int> ();
		mainScene = "MainMenu";
	}

	private static volatile SceneDatabase instance;
	private static object syncRoot = new Object();
	
	public static SceneDatabase getInstance{
		get {
			if (instance == null) {
				lock (syncRoot) {
					if (instance == null) 
						instance = new SceneDatabase();
				}
			}
			return instance;
		}
	}

	public void pushScene(int scene){
		if(!scenes.Contains(scene)){
			scenes.Push(scene);
		}
	}

	public int popScene(){
		return scenes.Pop ();
	}

	public void pushCategoryList(int categoryType){
		this.categoryType = categoryType;
	}

	public int getCategoryList(){
		return categoryType;
	}

	public void setMainScene(string mainScene){
		this.mainScene = mainScene;
	}

	public string getMainScene(){
		return mainScene;
	}

}