using System;

public class Model {
	private static Model instance;	
	private int levelQuantoEh;
	private int qtyChlQuantoEh;
	
	private Model (){
		levelQuantoEh = 1;
		qtyChlQuantoEh = 10;
	}

	public static Model getInstance()
	{
		if(instance == null){
			instance = new Model();
		}
		
		return instance;
	}
	/*public Model (){
		levelQuantoEh = 1;
		qtyChlQuantoEh = 10;
	}*/

	public int getLevelQuantoEh (){
		return levelQuantoEh;
	}

	public int getQtyChlQuantoEh (){
		return qtyChlQuantoEh;
	}

	public void incLevelQuantoEh (){
		levelQuantoEh++;
	}	
}
