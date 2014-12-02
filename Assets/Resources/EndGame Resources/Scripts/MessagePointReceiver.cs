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

		int userStars = user.StarsStage;

		int diffOtherAtempt = Math.Abs(user.StarsStage - value);

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
							"Vamos para a proximo subfase?";
						buttonPlayText.text = "PROXIMA SUBFASE!";
						user.releaseNextSubCategory();
					}else{
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta fase!\n" +
							"Vamos para a proximo fase?";
						buttonPlayText.text = "PROXIMA FASE!";
						user.releaseNextCategory();
					}

				}else{
					if(user.CurrentTask.Name != "Medir"){
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta fase!\n" +
							"Vamos para o proximo jogo?";
						buttonPlayText.text = "PROXIMO JOGO!";
						user.releaseNextTask();
					}else{
						messageEnd.text = "Parabens! \n voce ja ganhou todas as estrelas desta fase!\n" +
							"Vamos para a proxima categoria?";
						buttonPlayText.text = "PROXIMO categoria!";
						user.releaseNextCategory();
						user.CurrentStage++;
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

		if(user.StarsStage < value)
			user.StarsStage = value;

		pointValue.text = user.StarsStage.ToString();

	}

	/*
	 * @desc Call the scene according to the CurrentTask.Name
	 */
	public void GoBackScene(){
		Application.LoadLevel (user.CurrentTask.Name);
	}

	/*
	 * @desc Call the MainScene
	 */
	public void GoMainScene(){
		Application.LoadLevel ("MainMenu");
	}


}
