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

	private Image estrelaImage;

	private Text pointValue;
	private Text messageEnd;
	private Text buttonPlayText;
	private Image eugenioImage;
	private User user;
	private int value = -1; // doen't make sense. Unity almost obligated me to do this

	private string[] messagesFormat =
	{
		"Parabens! \n Voce acertou {0} e ganhou +{1} estrelas!",
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

		int diffOtherAtempt = Math.Abs(user.StarsStage - value);

		if(value >= 7){
			messageEnd.text = string.Format(messagesFormat[0], value, diffOtherAtempt);
			buttonPlayText.text = "JOGAR\nMAIS";
			eugenioImage.sprite = eugenioFeliz;

			if(value == 10){
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
					user.CurrentStage++;
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
	 * @desc Call tha last scene stored in the stack.
	 */
	public void GoBackScene(){
		Application.LoadLevel (user.CurrentTask.Name);
	}

	/*
	 * @desc Call the MainScene stored in the SceneDatabase
	 * @see SceneDatabase
	 */
	public void GoMainScene(){
		Application.LoadLevel ("MainMenu");
	}


}
