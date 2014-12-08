using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

/*
 * @desc Get the points achieve by the user in the task
 * deal with that to organize the scene.
 */
public class MessagePointReceiver : MonoBehaviour {

	public Sprite eugenioFeliz;
	public Sprite eugenioTriste;

	public Sprite estrelaOuro;
	public Sprite estrelaPrata;
	public Sprite estrelaBronze;

	Image estrelaImage;

	Text pointValue;
	Text messageEnd;
	Text buttonPlayText;
	Image eugenioImage;
	User user;
	int value = -1; // doen't make sense. Unity almost obligated me to do this

	string[] messagesFormat =
	{
		"Parabens! \n Voce acertou {0} desta e ganhou +{1} estrelas!",
		"Voce perdeu. \n Vamos tentar outra vez?"
	};

	/*
	 * @desc Get scene components that are going to be used
	 * Get UsetInstance.
	 */
	void Awake(){
		GameObject taskRunningGO = GameObject.FindGameObjectWithTag ("TIMELOADING"); // Do not erase this code.
		Destroy (taskRunningGO);

		pointValue = GameObject.Find("Value").GetComponent<Text> ();
		messageEnd = GameObject.Find ("MessageText").GetComponent<Text> ();
		buttonPlayText = GameObject.Find ("Text").GetComponent<Text> ();
		eugenioImage = GameObject.Find ("Eugenio").GetComponent<Image> ();
		estrelaImage = GameObject.Find ("Point").GetComponent<Image>();
		user = User.getInstance;
	}

	/*
	 * @desc Callback function called after Awake.
	 * Get ther user taskPoints. If he succeded the task
	 * increase his stage in one more
	 * 
	 */
	void Start () {

		value = user.TaskPoints;

		if(user.StarsStage < value){
			user.StarsStage = value;
		}

		int userStars = user.StarsStage;

		user.Stars_qty += userStars;

		int diffOtherAtempt = Math.Abs(userStars - value);

		if(userStars >= 7){
			messageEnd.text = string.Format(messagesFormat[0], value, diffOtherAtempt);
			buttonPlayText.text = "JOGAR\nMAIS";
			eugenioImage.sprite = eugenioFeliz;

			if(userStars == 10){
				messageEnd.text = string.Format(messagesFormat[0], value, 0); // He doesn't gain anything else if he already achieve the maximum value.

				estrelaImage.sprite = estrelaOuro;

				// Change this to use State Pattern

				if(user.CurrentTask.Name == "Adicao"){
					if(user.CurrentSubStage < 3){
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta subfase!\n" +
							"Vamos para a proxima?";
						buttonPlayText.text = "PROXIMA subfase!";
						user.releaseNextSubCategory();
					}else{
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta fase!\n" +
							"Vamos para a proxima?";
						buttonPlayText.text = "PROXIMA FASE!";
						user.CurrentStage++;
						user.releaseNextCategory();
					}

				}else{
					if(user.CurrentTask.Name != "Medir"){
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas deste jogo!\n" +
							"Vamos para o proximo?";
						buttonPlayText.text = "PROXIMO JOGO!";
						user.releaseNextTask();
					}else{
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta fase!\n" +
							"Vamos para a proxima?";
						buttonPlayText.text = "PROXIMO fase!";
						user.CurrentStage++;
						user.releaseNextCategory();

						if(user.Level_pet < ((MainCategory) user.CurrentCategory).Level){
							GoMainScene();
						}

					}
				}
			}else{
				estrelaImage.sprite = estrelaPrata;
			}

		}else{
			eugenioImage.sprite = eugenioTriste;
			messageEnd.text = messagesFormat[1];
			buttonPlayText.text = "JOGAR\nOUTRA VEZ";

			estrelaImage.sprite = estrelaBronze;

		}

		pointValue.text = user.StarsStage.ToString();

	}

	/*
	 * @desc Call the scene according to the CurrentTask.Name
	 */
	public void GoBackScene(){
		Application.LoadLevel (user.CurrentTask.Name);
		user.StartSavingTask ();

		GameObject persistenceTask = Resources.Load<GameObject> ("All_Task/prefab/TASK_RUNNING");
		Instantiate (persistenceTask);
	}

	/*
	 * @desc Call the MainScene
	 */
	public void GoMainScene(){
		Application.LoadLevel ("MainMenu");
	}


}
